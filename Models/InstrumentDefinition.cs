namespace OptionA.Tuner.Models;

public record InstrumentDefinition
{
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required IReadOnlyList<StringDefinition> Strings { get; init; }

    // ── Bowed Strings ──────────────────────────────────────────

    public static InstrumentDefinition Violin => new()
    {
        Name = "Violin",
        Category = "Bowed Strings",
        Strings =
        [
            new() { Name = "G", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "D", NoteName = "D", Octave = 4, MidiNote = 62 },
            new() { Name = "A", NoteName = "A", Octave = 4, MidiNote = 69 },
            new() { Name = "E", NoteName = "E", Octave = 5, MidiNote = 76 },
        ]
    };

    public static InstrumentDefinition Viola => new()
    {
        Name = "Viola",
        Category = "Bowed Strings",
        Strings =
        [
            new() { Name = "C", NoteName = "C", Octave = 3, MidiNote = 48 },
            new() { Name = "G", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "D", NoteName = "D", Octave = 4, MidiNote = 62 },
            new() { Name = "A", NoteName = "A", Octave = 4, MidiNote = 69 },
        ]
    };

    public static InstrumentDefinition Cello => new()
    {
        Name = "Cello",
        Category = "Bowed Strings",
        Strings =
        [
            new() { Name = "C", NoteName = "C", Octave = 2, MidiNote = 36 },
            new() { Name = "G", NoteName = "G", Octave = 2, MidiNote = 43 },
            new() { Name = "D", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "A", NoteName = "A", Octave = 3, MidiNote = 57 },
        ]
    };

    public static InstrumentDefinition DoubleBass => new()
    {
        Name = "Double Bass",
        Category = "Bowed Strings",
        Strings =
        [
            new() { Name = "E", NoteName = "E", Octave = 1, MidiNote = 28 },
            new() { Name = "A", NoteName = "A", Octave = 1, MidiNote = 33 },
            new() { Name = "D", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "G", NoteName = "G", Octave = 2, MidiNote = 43 },
        ]
    };

    // ── Guitar ─────────────────────────────────────────────────

    public static InstrumentDefinition GuitarStandard => new()
    {
        Name = "Guitar (Standard)",
        Category = "Guitar",
        Strings =
        [
            new() { Name = "E2", NoteName = "E", Octave = 2, MidiNote = 40 },
            new() { Name = "A2", NoteName = "A", Octave = 2, MidiNote = 45 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
        ]
    };

    public static InstrumentDefinition GuitarDropD => new()
    {
        Name = "Guitar (Drop D)",
        Category = "Guitar",
        Strings =
        [
            new() { Name = "D2", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "A2", NoteName = "A", Octave = 2, MidiNote = 45 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
        ]
    };

    public static InstrumentDefinition GuitarOpenG => new()
    {
        Name = "Guitar (Open G)",
        Category = "Guitar",
        Strings =
        [
            new() { Name = "D2", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "G2", NoteName = "G", Octave = 2, MidiNote = 43 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
        ]
    };

    public static InstrumentDefinition GuitarDADGAD => new()
    {
        Name = "Guitar (DADGAD)",
        Category = "Guitar",
        Strings =
        [
            new() { Name = "D2", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "A2", NoteName = "A", Octave = 2, MidiNote = 45 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "A3", NoteName = "A", Octave = 3, MidiNote = 57 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
        ]
    };

    public static InstrumentDefinition Guitar7String => new()
    {
        Name = "Guitar (7-String)",
        Category = "Guitar",
        Strings =
        [
            new() { Name = "B1", NoteName = "B", Octave = 1, MidiNote = 35 },
            new() { Name = "E2", NoteName = "E", Octave = 2, MidiNote = 40 },
            new() { Name = "A2", NoteName = "A", Octave = 2, MidiNote = 45 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
        ]
    };

    public static InstrumentDefinition Guitar12String => new()
    {
        Name = "Guitar (12-String)",
        Category = "Guitar",
        Strings =
        [
            new() { Name = "E2", NoteName = "E", Octave = 2, MidiNote = 40 },
            new() { Name = "A2", NoteName = "A", Octave = 2, MidiNote = 45 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
        ]
    };

    // ── Bass Guitar ────────────────────────────────────────────

    public static InstrumentDefinition BassGuitar4 => new()
    {
        Name = "Bass Guitar (4-String)",
        Category = "Bass Guitar",
        Strings =
        [
            new() { Name = "E1", NoteName = "E", Octave = 1, MidiNote = 28 },
            new() { Name = "A1", NoteName = "A", Octave = 1, MidiNote = 33 },
            new() { Name = "D2", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "G2", NoteName = "G", Octave = 2, MidiNote = 43 },
        ]
    };

    public static InstrumentDefinition BassGuitar5 => new()
    {
        Name = "Bass Guitar (5-String)",
        Category = "Bass Guitar",
        Strings =
        [
            new() { Name = "B0", NoteName = "B", Octave = 0, MidiNote = 23 },
            new() { Name = "E1", NoteName = "E", Octave = 1, MidiNote = 28 },
            new() { Name = "A1", NoteName = "A", Octave = 1, MidiNote = 33 },
            new() { Name = "D2", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "G2", NoteName = "G", Octave = 2, MidiNote = 43 },
        ]
    };

    public static InstrumentDefinition BassGuitarDropD => new()
    {
        Name = "Bass Guitar (Drop D)",
        Category = "Bass Guitar",
        Strings =
        [
            new() { Name = "D1", NoteName = "D", Octave = 1, MidiNote = 26 },
            new() { Name = "A1", NoteName = "A", Octave = 1, MidiNote = 33 },
            new() { Name = "D2", NoteName = "D", Octave = 2, MidiNote = 38 },
            new() { Name = "G2", NoteName = "G", Octave = 2, MidiNote = 43 },
        ]
    };

    // ── Ukulele ────────────────────────────────────────────────

    public static InstrumentDefinition UkuleleSoprano => new()
    {
        Name = "Ukulele (Soprano/Concert)",
        Category = "Ukulele",
        Strings =
        [
            new() { Name = "G4", NoteName = "G", Octave = 4, MidiNote = 67 },
            new() { Name = "C4", NoteName = "C", Octave = 4, MidiNote = 60 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
        ]
    };

    public static InstrumentDefinition UkuleleTenor => new()
    {
        Name = "Ukulele (Tenor)",
        Category = "Ukulele",
        Strings =
        [
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "C4", NoteName = "C", Octave = 4, MidiNote = 60 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
        ]
    };

    public static InstrumentDefinition UkuleleBaritone => new()
    {
        Name = "Ukulele (Baritone)",
        Category = "Ukulele",
        Strings =
        [
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
        ]
    };

    // ── Banjo ──────────────────────────────────────────────────

    public static InstrumentDefinition Banjo5String => new()
    {
        Name = "Banjo (5-String Open G)",
        Category = "Banjo",
        Strings =
        [
            new() { Name = "G4", NoteName = "G", Octave = 4, MidiNote = 67 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "B3", NoteName = "B", Octave = 3, MidiNote = 59 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
        ]
    };

    public static InstrumentDefinition Banjo4StringTenor => new()
    {
        Name = "Banjo (4-String Tenor)",
        Category = "Banjo",
        Strings =
        [
            new() { Name = "C3", NoteName = "C", Octave = 3, MidiNote = 48 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
        ]
    };

    // ── Mandolin ───────────────────────────────────────────────

    public static InstrumentDefinition Mandolin => new()
    {
        Name = "Mandolin",
        Category = "Mandolin",
        Strings =
        [
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
            new() { Name = "E5", NoteName = "E", Octave = 5, MidiNote = 76 },
        ]
    };

    public static InstrumentDefinition Mandola => new()
    {
        Name = "Mandola",
        Category = "Mandolin",
        Strings =
        [
            new() { Name = "C3", NoteName = "C", Octave = 3, MidiNote = 48 },
            new() { Name = "G3", NoteName = "G", Octave = 3, MidiNote = 55 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
        ]
    };

    // ── Other Strings ──────────────────────────────────────────

    public static InstrumentDefinition Charango => new()
    {
        Name = "Charango",
        Category = "Other Strings",
        Strings =
        [
            new() { Name = "G4", NoteName = "G", Octave = 4, MidiNote = 67 },
            new() { Name = "C5", NoteName = "C", Octave = 5, MidiNote = 72 },
            new() { Name = "E5", NoteName = "E", Octave = 5, MidiNote = 76 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
            new() { Name = "E5h", NoteName = "E", Octave = 5, MidiNote = 76 },
        ]
    };

    public static InstrumentDefinition Bouzouki => new()
    {
        Name = "Bouzouki (Irish)",
        Category = "Other Strings",
        Strings =
        [
            new() { Name = "G2", NoteName = "G", Octave = 2, MidiNote = 43 },
            new() { Name = "D3", NoteName = "D", Octave = 3, MidiNote = 50 },
            new() { Name = "A3", NoteName = "A", Octave = 3, MidiNote = 57 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
        ]
    };

    public static InstrumentDefinition Harp => new()
    {
        Name = "Lap Harp (Diatonic C)",
        Category = "Other Strings",
        Strings =
        [
            new() { Name = "C4", NoteName = "C", Octave = 4, MidiNote = 60 },
            new() { Name = "D4", NoteName = "D", Octave = 4, MidiNote = 62 },
            new() { Name = "E4", NoteName = "E", Octave = 4, MidiNote = 64 },
            new() { Name = "F4", NoteName = "F", Octave = 4, MidiNote = 65 },
            new() { Name = "G4", NoteName = "G", Octave = 4, MidiNote = 67 },
            new() { Name = "A4", NoteName = "A", Octave = 4, MidiNote = 69 },
            new() { Name = "B4", NoteName = "B", Octave = 4, MidiNote = 71 },
            new() { Name = "C5", NoteName = "C", Octave = 5, MidiNote = 72 },
        ]
    };

    // ── All instruments & categories ───────────────────────────

    public static IReadOnlyList<InstrumentDefinition> All =>
    [
        Violin, Viola, Cello, DoubleBass,
        GuitarStandard, GuitarDropD, GuitarOpenG, GuitarDADGAD, Guitar7String, Guitar12String,
        BassGuitar4, BassGuitar5, BassGuitarDropD,
        UkuleleSoprano, UkuleleTenor, UkuleleBaritone,
        Banjo5String, Banjo4StringTenor,
        Mandolin, Mandola,
        Charango, Bouzouki, Harp,
    ];

    public static IReadOnlyList<IGrouping<string, InstrumentDefinition>> Grouped =>
        All.GroupBy(i => i.Category).ToList();
}
