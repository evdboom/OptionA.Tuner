// OptionA Tuner — Audio Service
// Manages microphone capture via AudioWorklet (preferred) with
// AnalyserNode fallback for older browsers.

/** @typedef {(samples: Float32Array, sampleRate: number) => void} AudioCallback */

let audioContext = null;
let mediaStream = null;
let workletNode = null;
let analyser = null;
let fallbackTimerId = null;

const FALLBACK_TICK_MS = 33; // ~30 fps

/** @type {AudioCallback|null} */
let onBuffer = null;

/**
 * Check if the browser supports getUserMedia.
 * @returns {boolean}
 */
export function isSupported() {
    return !!(navigator.mediaDevices && navigator.mediaDevices.getUserMedia);
}

/**
 * Start capturing audio from the microphone.
 * @param {AudioCallback} callback  Called with (Float32Array, sampleRate) when a buffer is ready
 * @returns {Promise<number>}       The audio sample rate
 */
export async function start(callback) {
    onBuffer = callback;

    mediaStream = await navigator.mediaDevices.getUserMedia({
        audio: {
            echoCancellation: false,
            noiseSuppression: false,
            autoGainControl: false,
        },
    });

    audioContext = new AudioContext();
    if (audioContext.state === 'suspended') await audioContext.resume();

    const source = audioContext.createMediaStreamSource(mediaStream);
    const rate = audioContext.sampleRate;

    // Try AudioWorklet first (best performance — dedicated thread)
    if (audioContext.audioWorklet) {
        try {
            await audioContext.audioWorklet.addModule('./js/audio-worklet.js');
            workletNode = new AudioWorkletNode(audioContext, 'pitch-processor');
            workletNode.port.onmessage = (e) => {
                if (onBuffer) onBuffer(e.data, rate);
            };
            source.connect(workletNode);
            return rate;
        } catch (err) {
            console.warn('AudioWorklet failed, falling back to AnalyserNode:', err);
        }
    }

    // Fallback: AnalyserNode on main thread (~30 fps)
    analyser = audioContext.createAnalyser();
    analyser.fftSize = 4096;
    analyser.smoothingTimeConstant = 0;
    source.connect(analyser);

    const buf = new Float32Array(analyser.fftSize);
    const tick = () => {
        if (!analyser || !onBuffer) return;
        analyser.getFloatTimeDomainData(buf);
        onBuffer(buf, rate);
        fallbackTimerId = setTimeout(tick, FALLBACK_TICK_MS);
    };
    tick();

    return rate;
}

/**
 * Stop capturing and release all resources.
 */
export function stop() {
    onBuffer = null;

    if (fallbackTimerId != null) {
        clearTimeout(fallbackTimerId);
        fallbackTimerId = null;
    }

    if (workletNode) {
        workletNode.port.close();
        workletNode.disconnect();
        workletNode = null;
    }

    analyser = null;

    if (mediaStream) {
        mediaStream.getTracks().forEach((t) => t.stop());
        mediaStream = null;
    }

    if (audioContext) {
        audioContext.close();
        audioContext = null;
    }
}
