// OptionA Tuner — Instrument Definitions
// Ported from Models/InstrumentDefinition.cs

/** @typedef {{ name: string, noteName: string, octave: number, midiNote: number }} StringDef */
/** @typedef {{ name: string, category: string, strings: StringDef[] }} Instrument */

/** @type {Instrument[]} */
export const instruments = [
    // ── Bowed Strings ──────────────────────────────────────────
    {
        name: 'Violin', category: 'Bowed Strings', strings: [
            { name: 'G', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'D', noteName: 'D', octave: 4, midiNote: 62 },
            { name: 'A', noteName: 'A', octave: 4, midiNote: 69 },
            { name: 'E', noteName: 'E', octave: 5, midiNote: 76 },
        ]
    },
    {
        name: 'Viola', category: 'Bowed Strings', strings: [
            { name: 'C', noteName: 'C', octave: 3, midiNote: 48 },
            { name: 'G', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'D', noteName: 'D', octave: 4, midiNote: 62 },
            { name: 'A', noteName: 'A', octave: 4, midiNote: 69 },
        ]
    },
    {
        name: 'Cello', category: 'Bowed Strings', strings: [
            { name: 'C', noteName: 'C', octave: 2, midiNote: 36 },
            { name: 'G', noteName: 'G', octave: 2, midiNote: 43 },
            { name: 'D', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'A', noteName: 'A', octave: 3, midiNote: 57 },
        ]
    },
    {
        name: 'Double Bass', category: 'Bowed Strings', strings: [
            { name: 'E', noteName: 'E', octave: 1, midiNote: 28 },
            { name: 'A', noteName: 'A', octave: 1, midiNote: 33 },
            { name: 'D', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'G', noteName: 'G', octave: 2, midiNote: 43 },
        ]
    },

    // ── Guitar ─────────────────────────────────────────────────
    {
        name: 'Guitar (Standard)', category: 'Guitar', strings: [
            { name: 'E2', noteName: 'E', octave: 2, midiNote: 40 },
            { name: 'A2', noteName: 'A', octave: 2, midiNote: 45 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
        ]
    },
    {
        name: 'Guitar (Drop D)', category: 'Guitar', strings: [
            { name: 'D2', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'A2', noteName: 'A', octave: 2, midiNote: 45 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
        ]
    },
    {
        name: 'Guitar (Open G)', category: 'Guitar', strings: [
            { name: 'D2', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'G2', noteName: 'G', octave: 2, midiNote: 43 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
        ]
    },
    {
        name: 'Guitar (DADGAD)', category: 'Guitar', strings: [
            { name: 'D2', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'A2', noteName: 'A', octave: 2, midiNote: 45 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'A3', noteName: 'A', octave: 3, midiNote: 57 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
        ]
    },
    {
        name: 'Guitar (7-String)', category: 'Guitar', strings: [
            { name: 'B1', noteName: 'B', octave: 1, midiNote: 35 },
            { name: 'E2', noteName: 'E', octave: 2, midiNote: 40 },
            { name: 'A2', noteName: 'A', octave: 2, midiNote: 45 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
        ]
    },
    {
        name: 'Guitar (12-String)', category: 'Guitar', strings: [
            { name: 'E2', noteName: 'E', octave: 2, midiNote: 40 },
            { name: 'A2', noteName: 'A', octave: 2, midiNote: 45 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
        ]
    },

    // ── Bass Guitar ────────────────────────────────────────────
    {
        name: 'Bass Guitar (4-String)', category: 'Bass Guitar', strings: [
            { name: 'E1', noteName: 'E', octave: 1, midiNote: 28 },
            { name: 'A1', noteName: 'A', octave: 1, midiNote: 33 },
            { name: 'D2', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'G2', noteName: 'G', octave: 2, midiNote: 43 },
        ]
    },
    {
        name: 'Bass Guitar (5-String)', category: 'Bass Guitar', strings: [
            { name: 'B0', noteName: 'B', octave: 0, midiNote: 23 },
            { name: 'E1', noteName: 'E', octave: 1, midiNote: 28 },
            { name: 'A1', noteName: 'A', octave: 1, midiNote: 33 },
            { name: 'D2', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'G2', noteName: 'G', octave: 2, midiNote: 43 },
        ]
    },
    {
        name: 'Bass Guitar (Drop D)', category: 'Bass Guitar', strings: [
            { name: 'D1', noteName: 'D', octave: 1, midiNote: 26 },
            { name: 'A1', noteName: 'A', octave: 1, midiNote: 33 },
            { name: 'D2', noteName: 'D', octave: 2, midiNote: 38 },
            { name: 'G2', noteName: 'G', octave: 2, midiNote: 43 },
        ]
    },

    // ── Ukulele ────────────────────────────────────────────────
    {
        name: 'Ukulele (Soprano/Concert)', category: 'Ukulele', strings: [
            { name: 'G4', noteName: 'G', octave: 4, midiNote: 67 },
            { name: 'C4', noteName: 'C', octave: 4, midiNote: 60 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
        ]
    },
    {
        name: 'Ukulele (Tenor)', category: 'Ukulele', strings: [
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'C4', noteName: 'C', octave: 4, midiNote: 60 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
        ]
    },
    {
        name: 'Ukulele (Baritone)', category: 'Ukulele', strings: [
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
        ]
    },

    // ── Banjo ──────────────────────────────────────────────────
    {
        name: 'Banjo (5-String Open G)', category: 'Banjo', strings: [
            { name: 'G4', noteName: 'G', octave: 4, midiNote: 67 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'B3', noteName: 'B', octave: 3, midiNote: 59 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
        ]
    },
    {
        name: 'Banjo (4-String Tenor)', category: 'Banjo', strings: [
            { name: 'C3', noteName: 'C', octave: 3, midiNote: 48 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
        ]
    },

    // ── Mandolin ───────────────────────────────────────────────
    {
        name: 'Mandolin', category: 'Mandolin', strings: [
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
            { name: 'E5', noteName: 'E', octave: 5, midiNote: 76 },
        ]
    },
    {
        name: 'Mandola', category: 'Mandolin', strings: [
            { name: 'C3', noteName: 'C', octave: 3, midiNote: 48 },
            { name: 'G3', noteName: 'G', octave: 3, midiNote: 55 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
        ]
    },

    // ── Other Strings ──────────────────────────────────────────
    {
        name: 'Charango', category: 'Other Strings', strings: [
            { name: 'G4', noteName: 'G', octave: 4, midiNote: 67 },
            { name: 'C5', noteName: 'C', octave: 5, midiNote: 72 },
            { name: 'E5', noteName: 'E', octave: 5, midiNote: 76 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
            { name: 'E5h', noteName: 'E', octave: 5, midiNote: 76 },
        ]
    },
    {
        name: 'Bouzouki (Irish)', category: 'Other Strings', strings: [
            { name: 'G2', noteName: 'G', octave: 2, midiNote: 43 },
            { name: 'D3', noteName: 'D', octave: 3, midiNote: 50 },
            { name: 'A3', noteName: 'A', octave: 3, midiNote: 57 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
        ]
    },
    {
        name: 'Lap Harp (Diatonic C)', category: 'Other Strings', strings: [
            { name: 'C4', noteName: 'C', octave: 4, midiNote: 60 },
            { name: 'D4', noteName: 'D', octave: 4, midiNote: 62 },
            { name: 'E4', noteName: 'E', octave: 4, midiNote: 64 },
            { name: 'F4', noteName: 'F', octave: 4, midiNote: 65 },
            { name: 'G4', noteName: 'G', octave: 4, midiNote: 67 },
            { name: 'A4', noteName: 'A', octave: 4, midiNote: 69 },
            { name: 'B4', noteName: 'B', octave: 4, midiNote: 71 },
            { name: 'C5', noteName: 'C', octave: 5, midiNote: 72 },
        ]
    },
];

/**
 * Group instruments by category.
 * @returns {Map<string, Instrument[]>}
 */
export function getGrouped() {
    /** @type {Map<string, Instrument[]>} */
    const map = new Map();
    for (const inst of instruments) {
        if (!map.has(inst.category)) map.set(inst.category, []);
        map.get(inst.category).push(inst);
    }
    return map;
}

/**
 * Get the target frequency for a string at a given A4 reference.
 * @param {StringDef} str
 * @param {number} referenceA4
 * @returns {number}
 */
export function getStringFrequency(str, referenceA4) {
    return referenceA4 * Math.pow(2, (str.midiNote - 69) / 12);
}
