namespace OptionA.Tuner.Services;

/// <summary>
/// Pitch detector using the McLeod Pitch Method (MPM).
/// Optimized for monophonic instruments in the cello range (C2 ~65Hz to ~1kHz).
/// </summary>
public static class PitchDetector
{
    private const double DefaultClarityThreshold = 0.80;
    private const double NoiseFloorRms = 0.01;

    /// <summary>
    /// Detect the fundamental pitch of the audio buffer
    /// </summary>
    /// <param name="samples">Audio samples (Float32, range -1..1)</param>
    /// <param name="sampleRate">Sample rate in Hz (e.g., 44100)</param>
    /// <param name="minFrequency">Minimum detectable frequency in Hz</param>
    /// <param name="maxFrequency">Maximum detectable frequency in Hz</param>
    /// <returns>Detected frequency in Hz, or null if no pitch detected</returns>
    public static double? DetectPitch(
        float[] samples,
        int sampleRate,
        double minFrequency = 50.0,
        double maxFrequency = 1200.0)
    {
        if (samples.Length == 0)
        {
            return null;
        }

        // Noise gate: check RMS level
        var rms = CalculateRms(samples);
        if (rms < NoiseFloorRms)
        {
            return null;
        }

        var minLag = (int)(sampleRate / maxFrequency);
        var maxLag = (int)(sampleRate / minFrequency);

        // Clamp maxLag to half the buffer length
        maxLag = Math.Min(maxLag, samples.Length / 2);

        if (minLag >= maxLag)
        {
            return null;
        }

        // Compute the Normalized Square Difference Function (NSDF)
        var nsdf = ComputeNsdf(samples, maxLag);

        // Find peaks in NSDF
        var bestLag = FindBestPeak(nsdf, minLag, maxLag);

        if (bestLag < 0)
        {
            return null;
        }

        // Parabolic interpolation for sub-sample accuracy
        var interpolatedLag = ParabolicInterpolation(nsdf, bestLag);

        if (interpolatedLag <= 0)
        {
            return null;
        }

        var frequency = sampleRate / interpolatedLag;

        // Final sanity check
        if (frequency < minFrequency || frequency > maxFrequency)
        {
            return null;
        }

        return frequency;
    }

    /// <summary>
    /// Compute the Normalized Square Difference Function (NSDF)
    /// NSDF(τ) = 2 * r(τ) / m(τ)
    /// where r(τ) is the autocorrelation and m(τ) is the normalization term
    /// </summary>
    private static double[] ComputeNsdf(float[] samples, int maxLag)
    {
        var n = samples.Length;
        var nsdf = new double[maxLag + 1];

        for (var tau = 0; tau <= maxLag; tau++)
        {
            double acf = 0; // autocorrelation
            double energy = 0; // normalization (sum of squares)

            for (var i = 0; i < n - tau; i++)
            {
                acf += samples[i] * samples[i + tau];
                energy += samples[i] * samples[i] + samples[i + tau] * samples[i + tau];
            }

            nsdf[tau] = energy > 0 ? 2.0 * acf / energy : 0;
        }

        return nsdf;
    }

    /// <summary>
    /// Find the best peak in the NSDF above the clarity threshold.
    /// Uses the McLeod method: find all peaks, select the first one
    /// that exceeds the threshold relative to the global maximum.
    /// </summary>
    private static int FindBestPeak(double[] nsdf, int minLag, int maxLag)
    {
        // Find all positive-going zero crossings and subsequent peaks
        var peaks = new List<(int lag, double value)>();
        var isNegative = true;
        var currentPeakLag = -1;
        var currentPeakValue = double.MinValue;

        for (var i = minLag; i <= maxLag; i++)
        {
            if (nsdf[i] < 0)
            {
                if (!isNegative && currentPeakLag >= 0)
                {
                    peaks.Add((currentPeakLag, currentPeakValue));
                }
                isNegative = true;
                currentPeakLag = -1;
                currentPeakValue = double.MinValue;
            }
            else
            {
                isNegative = false;
                if (nsdf[i] > currentPeakValue)
                {
                    currentPeakValue = nsdf[i];
                    currentPeakLag = i;
                }
            }
        }

        // Don't forget the last peak if we ended in positive territory
        if (!isNegative && currentPeakLag >= 0)
        {
            peaks.Add((currentPeakLag, currentPeakValue));
        }

        if (peaks.Count == 0)
        {
            return -1;
        }

        // Find the maximum peak value
        var maxPeakValue = double.MinValue;
        foreach (var (_, value) in peaks)
        {
            if (value > maxPeakValue)
            {
                maxPeakValue = value;
            }
        }

        // Select the first peak above the clarity threshold
        var threshold = maxPeakValue * DefaultClarityThreshold;

        foreach (var (lag, value) in peaks)
        {
            if (value >= threshold)
            {
                return lag;
            }
        }

        return -1;
    }

    /// <summary>
    /// Apply parabolic interpolation around a peak for sub-sample accuracy
    /// </summary>
    private static double ParabolicInterpolation(double[] data, int peakIndex)
    {
        if (peakIndex <= 0 || peakIndex >= data.Length - 1)
        {
            return peakIndex;
        }

        var alpha = data[peakIndex - 1];
        var beta = data[peakIndex];
        var gamma = data[peakIndex + 1];

        var denominator = 2.0 * (2.0 * beta - alpha - gamma);

        if (Math.Abs(denominator) < 1e-10)
        {
            return peakIndex;
        }

        var delta = (alpha - gamma) / denominator;

        return peakIndex + delta;
    }

    /// <summary>
    /// Calculate the Root Mean Square of the signal
    /// </summary>
    private static double CalculateRms(float[] samples)
    {
        double sum = 0;
        for (var i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        return Math.Sqrt(sum / samples.Length);
    }
}
