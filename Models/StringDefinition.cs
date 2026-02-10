namespace OptionA.Tuner.Models;

public record StringDefinition
{
    /// <summary>
    /// Display name for the string (e.g., "C", "G", "D", "A")
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The note name this string should be tuned to
    /// </summary>
    public required string NoteName { get; init; }

    /// <summary>
    /// The octave for this string's note
    /// </summary>
    public required int Octave { get; init; }

    /// <summary>
    /// MIDI note number for this string
    /// </summary>
    public required int MidiNote { get; init; }

    /// <summary>
    /// Display name including octave (e.g., "C2")
    /// </summary>
    public string DisplayName => $"{NoteName}{Octave}";

    /// <summary>
    /// Get the target frequency for this string given a reference A4 pitch
    /// </summary>
    public double GetFrequency(double referenceA4)
    {
        return referenceA4 * Math.Pow(2.0, (MidiNote - 69) / 12.0);
    }
}
