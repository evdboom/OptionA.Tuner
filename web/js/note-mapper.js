// OptionA Tuner — Note Mapper (12-TET)
// Ported from Services/NoteMapper.cs

const NOTE_NAMES = ['C', 'C#', 'D', 'D#', 'E', 'F', 'F#', 'G', 'G#', 'A', 'A#', 'B'];

/**
 * @typedef {Object} NoteInfo
 * @property {string} name        Note name (e.g. "A", "C#")
 * @property {number} octave      Octave number
 * @property {number} centsOffset Cents offset from perfect pitch (-50..+50)
 * @property {number} targetFreq  Exact frequency of the nearest 12-TET note
 * @property {number} detectedFreq Actual detected frequency
 * @property {number} midiNote    MIDI note number (A4 = 69)
 * @property {boolean} isInTune   |cents| <= 5
 * @property {boolean} isSharp    cents > 5
 * @property {boolean} isFlat     cents < -5
 */

/**
 * Map a detected frequency to the nearest 12-TET note.
 * @param {number} frequency    Detected frequency in Hz
 * @param {number} [referenceA4=440] Reference pitch for A4
 * @returns {NoteInfo}
 */
export function mapNote(frequency, referenceA4 = 440) {
    const semitonesFromA4 = 12.0 * Math.log2(frequency / referenceA4);
    const midiExact = 69.0 + semitonesFromA4;
    const midiRounded = Math.round(midiExact);

    let centsOffset = (midiExact - midiRounded) * 100.0;
    centsOffset = Math.max(-50, Math.min(50, centsOffset));

    const noteIndex = ((midiRounded % 12) + 12) % 12;
    const octave = Math.floor(midiRounded / 12) - 1;
    const targetFreq = referenceA4 * Math.pow(2, (midiRounded - 69) / 12);

    return {
        name: NOTE_NAMES[noteIndex],
        octave,
        centsOffset,
        targetFreq,
        detectedFreq: frequency,
        midiNote: midiRounded,
        isInTune: Math.abs(centsOffset) <= 5,
        isSharp: centsOffset > 5,
        isFlat: centsOffset < -5,
    };
}
