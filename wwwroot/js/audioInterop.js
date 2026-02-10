let audioContext = null;
let analyser = null;
let mediaStream = null;
let animationFrameId = null;
let dotNetHelper = null;
let isRunning = false;

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

        // Resume context if suspended (browser autoplay policy)
        if (audioContext.state === 'suspended') {
            await audioContext.resume();
        }

        const source = audioContext.createMediaStreamSource(mediaStream);
        analyser = audioContext.createAnalyser();
        analyser.fftSize = 4096;
        analyser.smoothingTimeConstant = 0;
        source.connect(analyser);

        isRunning = true;
        tick();

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

    // Convert to regular array for JS interop
    dotNetHelper.invokeMethodAsync('ReceiveAudioData', Array.from(buffer));

    animationFrameId = requestAnimationFrame(tick);
}

export function stopMicrophone() {
    isRunning = false;

    if (animationFrameId) {
        cancelAnimationFrame(animationFrameId);
        animationFrameId = null;
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
    return !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
}
