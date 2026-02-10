using OptionA.Tuner.Models;

namespace OptionA.Tuner.Services;

public static class NoteMapper
{
    /// <summary>
    /// Maps a detected frequency to the nearest note, computing cents offset
    /// </summary>
    /// <param name="frequency">Detected frequency in Hz</param>
    /// <param name="referenceA4">Reference pitch for A4 (e.g., 440.0)</param>
    /// <returns>NoteInfo with note name, octave, and cents offset</returns>
    public static NoteInfo Map(double frequency, double referenceA4 = 440.0)
    {
        // Calculate the number of semitones from A4
        var semitonesFromA4 = 12.0 * Math.Log2(frequency / referenceA4);

        // MIDI note number (A4 = 69)
        var midiNoteExact = 69.0 + semitonesFromA4;
        var midiNoteRounded = (int)Math.Round(midiNoteExact);

        // Cents offset from the nearest note
        var centsOffset = (midiNoteExact - midiNoteRounded) * 100.0;

        // Clamp cents to -50..+50
        centsOffset = Math.Clamp(centsOffset, -50.0, 50.0);

        // Note name and octave from MIDI note number
        // MIDI note 0 = C-1, so octave = (midiNote / 12) - 1
        var noteIndex = ((midiNoteRounded % 12) + 12) % 12;
        var octave = (midiNoteRounded / 12) - 1;

        // Target frequency for the nearest note
        var targetFrequency = referenceA4 * Math.Pow(2.0, (midiNoteRounded - 69) / 12.0);

        return new NoteInfo
        {
            Name = NoteInfo.NoteNames[noteIndex],
            Octave = octave,
            CentsOffset = centsOffset,
            TargetFrequency = targetFrequency,
            DetectedFrequency = frequency,
            MidiNote = midiNoteRounded
        };
    }
}
