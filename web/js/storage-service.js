// OptionA Tuner — Local Storage Service
// Ported from Services/LocalStorageService.cs

const INSTRUMENT_KEY = 'tuner_instrument';
const REFERENCE_A4_KEY = 'tuner_referenceA4';

/** @returns {string|null} */
export function getInstrument() {
    return localStorage.getItem(INSTRUMENT_KEY);
}

/** @param {string} name */
export function setInstrument(name) {
    localStorage.setItem(INSTRUMENT_KEY, name);
}

/** @returns {number|null} */
export function getReferenceA4() {
    const v = localStorage.getItem(REFERENCE_A4_KEY);
    if (v === null) return null;
    const n = parseInt(v, 10);
    return Number.isFinite(n) ? n : null;
}

/** @param {number} value */
export function setReferenceA4(value) {
    localStorage.setItem(REFERENCE_A4_KEY, String(value));
}
