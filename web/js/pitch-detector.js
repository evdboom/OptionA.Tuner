// OptionA Tuner — Pitch Detector (McLeod Pitch Method)
// Ported from Services/PitchDetector.cs

const CLARITY_THRESHOLD = 0.80;
const NOISE_FLOOR_RMS = 0.01;

/**
 * Detect the fundamental pitch of an audio buffer.
 * @param {Float32Array} samples  Audio samples (-1..1)
 * @param {number} sampleRate     Sample rate in Hz (e.g. 44100)
 * @param {number} [minFreq=50]   Minimum detectable frequency
 * @param {number} [maxFreq=1200] Maximum detectable frequency
 * @returns {number|null}         Detected frequency in Hz, or null
 */
export function detectPitch(samples, sampleRate, minFreq = 50, maxFreq = 1200) {
    const n = samples.length;
    if (n === 0) return null;

    // Noise gate: check RMS
    if (calculateRms(samples, n) < NOISE_FLOOR_RMS) return null;

    let minLag = Math.trunc(sampleRate / maxFreq);
    let maxLag = Math.trunc(sampleRate / minFreq);
    maxLag = Math.min(maxLag, n >>> 1); // clamp to half buffer

    if (minLag >= maxLag) return null;

    // Compute the Normalized Square Difference Function (NSDF)
    const nsdf = computeNsdf(samples, n, maxLag);

    // Find best peak
    const bestLag = findBestPeak(nsdf, minLag, maxLag);
    if (bestLag < 0) return null;

    // Parabolic interpolation for sub-sample accuracy
    const interpolated = parabolicInterpolation(nsdf, bestLag);
    if (interpolated <= 0) return null;

    const frequency = sampleRate / interpolated;
    if (frequency < minFreq || frequency > maxFreq) return null;

    return frequency;
}

/**
 * NSDF(τ) = 2·r(τ) / m(τ)
 * r(τ) = autocorrelation, m(τ) = energy normalization
 */
function computeNsdf(samples, n, maxLag) {
    const nsdf = new Float64Array(maxLag + 1);

    for (let tau = 0; tau <= maxLag; tau++) {
        let acf = 0;
        let energy = 0;
        const limit = n - tau;

        for (let i = 0; i < limit; i++) {
            const si = samples[i];
            const sit = samples[i + tau];
            acf += si * sit;
            energy += si * si + sit * sit;
        }

        nsdf[tau] = energy > 0 ? (2.0 * acf) / energy : 0;
    }

    return nsdf;
}

/**
 * Find the first peak above the clarity threshold (McLeod method).
 */
function findBestPeak(nsdf, minLag, maxLag) {
    const peaks = [];
    let isNegative = true;
    let currentPeakLag = -1;
    let currentPeakVal = -Infinity;

    for (let i = minLag; i <= maxLag; i++) {
        if (nsdf[i] < 0) {
            if (!isNegative && currentPeakLag >= 0) {
                peaks.push(currentPeakLag, currentPeakVal);
            }
            isNegative = true;
            currentPeakLag = -1;
            currentPeakVal = -Infinity;
        } else {
            isNegative = false;
            if (nsdf[i] > currentPeakVal) {
                currentPeakVal = nsdf[i];
                currentPeakLag = i;
            }
        }
    }

    // Last peak if still in positive territory
    if (!isNegative && currentPeakLag >= 0) {
        peaks.push(currentPeakLag, currentPeakVal);
    }

    if (peaks.length === 0) return -1;

    // Find max peak value (peaks is flat: [lag, val, lag, val, ...])
    let maxVal = -Infinity;
    for (let i = 1; i < peaks.length; i += 2) {
        if (peaks[i] > maxVal) maxVal = peaks[i];
    }

    const threshold = maxVal * CLARITY_THRESHOLD;

    // First peak above threshold
    for (let i = 0; i < peaks.length; i += 2) {
        if (peaks[i + 1] >= threshold) return peaks[i];
    }

    return -1;
}

/**
 * Parabolic interpolation around a peak for sub-sample accuracy.
 */
function parabolicInterpolation(data, peakIndex) {
    if (peakIndex <= 0 || peakIndex >= data.length - 1) return peakIndex;

    const alpha = data[peakIndex - 1];
    const beta = data[peakIndex];
    const gamma = data[peakIndex + 1];
    const denom = 2.0 * (2.0 * beta - alpha - gamma);

    if (Math.abs(denom) < 1e-10) return peakIndex;

    return peakIndex + (alpha - gamma) / denom;
}

/**
 * Root Mean Square of the signal.
 */
function calculateRms(samples, n) {
    let sum = 0;
    for (let i = 0; i < n; i++) {
        sum += samples[i] * samples[i];
    }
    return Math.sqrt(sum / n);
}
