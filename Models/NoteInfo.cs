namespace OptionA.Tuner.Models;

public record NoteInfo
{
    public static readonly string[] NoteNames = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"];

    /// <summary>
    /// Note name (e.g., "A", "C#")
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Octave number (e.g., 3 for A3)
    /// </summary>
    public required int Octave { get; init; }

    /// <summary>
    /// Cents offset from the perfect pitch (-50 to +50)
    /// </summary>
    public required double CentsOffset { get; init; }

    /// <summary>
    /// The exact frequency of the nearest note in the current tuning
    /// </summary>
    public required double TargetFrequency { get; init; }

    /// <summary>
    /// The actual detected frequency
    /// </summary>
    public required double DetectedFrequency { get; init; }

    /// <summary>
    /// MIDI note number (A4 = 69)
    /// </summary>
    public required int MidiNote { get; init; }

    /// <summary>
    /// Full display name, e.g. "A3"
    /// </summary>
    public string DisplayName => $"{Name}{Octave}";

    /// <summary>
    /// Whether the note is considered in tune (within ±5 cents)
    /// </summary>
    public bool IsInTune => Math.Abs(CentsOffset) <= 5.0;

    /// <summary>
    /// Whether the note is sharp (positive cents offset)
    /// </summary>
    public bool IsSharp => CentsOffset > 5.0;

    /// <summary>
    /// Whether the note is flat (negative cents offset)
    /// </summary>
    public bool IsFlat => CentsOffset < -5.0;
}
