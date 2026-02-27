let audioContext = null;
let analyser = null;
let mediaStream = null;
let timerId = null;
let workletNode = null;
let dotNetHelper = null;
let isRunning = false;

// ~30 fps throttle interval (used by AnalyserNode fallback only)
const TICK_INTERVAL_MS = 33;

function sendBuffer(floatBuffer) {
    if (!dotNetHelper) return;
    const bytes = new Uint8Array(floatBuffer.buffer);
    dotNetHelper.invokeMethodAsync('ReceiveAudioData', bytes);
}

export async function startMicrophone(objRef) {
    dotNetHelper = objRef;

    try {
        mediaStream = await navigator.mediaDevices.getUserMedia({
            audio: {
                echoCancellation: false,
                noiseSuppression: false,
                autoGainControl: false
            }
        });

        audioContext = new AudioContext();

        if (audioContext.state === 'suspended') {
            await audioContext.resume();
        }

        const source = audioContext.createMediaStreamSource(mediaStream);

        // Prefer AudioWorklet (off main thread) with AnalyserNode fallback
        let useWorklet = false;
        if (audioContext.audioWorklet) {
            try {
                await audioContext.audioWorklet.addModule('js/pitchProcessor.js');
                useWorklet = true;
            } catch (e) {
                console.warn('AudioWorklet unavailable, falling back to AnalyserNode:', e);
            }
        }

        if (useWorklet) {
            workletNode = new AudioWorkletNode(audioContext, 'pitch-processor');
            source.connect(workletNode);
            workletNode.port.onmessage = (e) => {
                if (!isRunning) return;
                sendBuffer(e.data);
            };
        } else {
            analyser = audioContext.createAnalyser();
            analyser.fftSize = 4096;
            analyser.smoothingTimeConstant = 0;
            source.connect(analyser);
        }

        isRunning = true;

        // Only start the polling loop for the fallback path
        if (!useWorklet) {
            tick();
        }

        return audioContext.sampleRate;
    } catch (err) {
        console.error('Microphone access error:', err);
        throw err;
    }
}

function tick() {
    if (!isRunning || !analyser || !dotNetHelper) return;

    const buffer = new Float32Array(analyser.fftSize);
    analyser.getFloatTimeDomainData(buffer);
    sendBuffer(buffer);

    timerId = setTimeout(tick, TICK_INTERVAL_MS);
}

export function stopMicrophone() {
    isRunning = false;

    if (timerId) {
        clearTimeout(timerId);
        timerId = null;
    }

    if (workletNode) {
        workletNode.port.onmessage = null;
        workletNode.disconnect();
        workletNode = null;
    }

    if (mediaStream) {
        mediaStream.getTracks().forEach(track => track.stop());
        mediaStream = null;
    }

    if (audioContext) {
        audioContext.close();
        audioContext = null;
    }

    analyser = null;
    dotNetHelper = null;
}

export function isSupported() {
    return !!navigator.mediaDevices?.getUserMedia;
}
