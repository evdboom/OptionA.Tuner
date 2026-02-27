// OptionA Tuner — String Indicator Component
// Ported from Components/StringIndicator.razor

import { LitElement, html, css } from '../vendor/lit.js';
import { getStringFrequency } from '../js/instruments.js';

export class StringIndicator extends LitElement {
    static properties = {
        /** @type {import('../js/instruments.js').Instrument|null} */
        instrument: { type: Object },
        /** @type {import('../js/note-mapper.js').NoteInfo|null} */
        currentNote: { type: Object },
        isActive: { type: Boolean },
        referenceA4: { type: Number },
    };

    static styles = css`
        :host { display: block; }

        .string-indicator {
            display: flex;
            gap: 0.75rem;
            justify-content: center;
            padding: 0.75rem 0;
        }

        .string-button {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            width: 56px;
            height: 56px;
            border: 2px solid var(--border-color);
            border-radius: var(--radius);
            background: var(--bg-card);
            color: var(--text-secondary);
            cursor: default;
            transition: all var(--transition);
            position: relative;
        }

        .string-button.closest {
            border-color: var(--color-intune);
            background: rgba(78, 205, 196, 0.1);
            color: var(--color-intune);
            box-shadow: 0 0 12px rgba(78, 205, 196, 0.3);
        }

        .string-note {
            font-size: 1.3rem;
            font-weight: 700;
            line-height: 1;
        }

        .string-octave {
            font-size: 0.65rem;
            opacity: 0.7;
            line-height: 1;
        }

        @media (max-width: 360px) {
            .string-button { width: 48px; height: 48px; }
            .string-note   { font-size: 1.1rem; }
        }
    `;

    constructor() {
        super();
        this.instrument = null;
        this.currentNote = null;
        this.isActive = false;
        this.referenceA4 = 440;
    }

    _isNearestString(str) {
        if (!this.currentNote) return false;
        const detected = this.currentNote.midiNote;
        let nearest = null;
        let smallestDist = Infinity;

        for (const s of this.instrument.strings) {
            const dist = Math.abs(s.midiNote - detected);
            if (dist < smallestDist) {
                smallestDist = dist;
                nearest = s;
            }
        }

        return nearest !== null && nearest.midiNote === str.midiNote;
    }

    render() {
        if (!this.instrument) return html``;

        // Reverse order to match the Blazor version
        const reversed = [...this.instrument.strings].reverse();

        return html`
            <div class="string-indicator">
                ${reversed.map((str) => {
                    const isClosest = this.isActive && this.currentNote && this._isNearestString(str);
                    const freq = getStringFrequency(str, this.referenceA4);
                    return html`
                        <div class="string-button ${isClosest ? 'closest' : ''}"
                             title="${str.noteName}${str.octave} (${freq.toFixed(1)} Hz)">
                            <span class="string-note">${str.name}</span>
                            <span class="string-octave">${str.octave}</span>
                        </div>
                    `;
                })}
            </div>
        `;
    }
}

customElements.define('string-indicator', StringIndicator);
