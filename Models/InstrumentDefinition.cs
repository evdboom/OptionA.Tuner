namespace OptionA.Tuner.Models;

public record InstrumentDefinition
{
    public required string Name { get; init; }
    public required IReadOnlyList<StringDefinition> Strings { get; init; }

    public static InstrumentDefinition Cello => new()
    {
        Name = "Cello",
        Strings =
        [
            new StringDefinition { Name = "C", NoteName = "C", Octave = 2, MidiNote = 36 },
            new StringDefinition { Name = "G", NoteName = "G", Octave = 2, MidiNote = 43 },
            new StringDefinition { Name = "D", NoteName = "D", Octave = 3, MidiNote = 50 },
            new StringDefinition { Name = "A", NoteName = "A", Octave = 3, MidiNote = 57 },
        ]
    };

    public static InstrumentDefinition Violin => new()
    {
        Name = "Violin",
        Strings =
        [
            new StringDefinition { Name = "G", NoteName = "G", Octave = 3, MidiNote = 55 },
            new StringDefinition { Name = "D", NoteName = "D", Octave = 4, MidiNote = 62 },
            new StringDefinition { Name = "A", NoteName = "A", Octave = 4, MidiNote = 69 },
            new StringDefinition { Name = "E", NoteName = "E", Octave = 5, MidiNote = 76 },
        ]
    };

    public static InstrumentDefinition Viola => new()
    {
        Name = "Viola",
        Strings =
        [
            new StringDefinition { Name = "C", NoteName = "C", Octave = 3, MidiNote = 48 },
            new StringDefinition { Name = "G", NoteName = "G", Octave = 3, MidiNote = 55 },
            new StringDefinition { Name = "D", NoteName = "D", Octave = 4, MidiNote = 62 },
            new StringDefinition { Name = "A", NoteName = "A", Octave = 4, MidiNote = 69 },
        ]
    };

    public static InstrumentDefinition Bass => new()
    {
        Name = "Double Bass",
        Strings =
        [
            new StringDefinition { Name = "E", NoteName = "E", Octave = 1, MidiNote = 28 },
            new StringDefinition { Name = "A", NoteName = "A", Octave = 1, MidiNote = 33 },
            new StringDefinition { Name = "D", NoteName = "D", Octave = 2, MidiNote = 38 },
            new StringDefinition { Name = "G", NoteName = "G", Octave = 2, MidiNote = 43 },
        ]
    };

    public static IReadOnlyList<InstrumentDefinition> All => [Cello, Violin, Viola, Bass];
}
