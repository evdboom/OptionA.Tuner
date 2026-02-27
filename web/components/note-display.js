// OptionA Tuner — Note Display Component
// Ported from Components/NoteDisplay.razor

import { LitElement, html, css, nothing } from '../vendor/lit.js';

export class NoteDisplay extends LitElement {
    static properties = {
        /** @type {import('../js/note-mapper.js').NoteInfo|null} */
        note: { type: Object },
        isActive: { type: Boolean },
    };

    static styles = css`
        :host {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center;
            padding: 1rem 0;
            min-height: 100px;
        }

        .note-name-row {
            display: flex;
            align-items: baseline;
            gap: 0.1rem;
            justify-content: center;
        }

        .note-name {
            font-size: 4.5rem;
            font-weight: 700;
            line-height: 1;
            color: var(--text-primary);
        }
        .note-name.muted { color: var(--text-muted); }

        .note-octave {
            font-size: 1.8rem;
            font-weight: 400;
            color: var(--text-secondary);
            align-self: flex-end;
            margin-bottom: 0.3rem;
        }

        .accidental-indicator {
            width: 2.5rem;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .accidental-indicator.left  { justify-content: flex-end; }
        .accidental-indicator.right { justify-content: flex-start; }

        .indicator { font-size: 2rem; font-weight: 700; }
        .indicator.flat  { color: var(--color-flat); }
        .indicator.sharp { color: var(--color-sharp); }
        .indicator.intune { color: var(--color-intune); font-size: 1.8rem; }

        .frequency-display {
            font-size: 0.85rem;
            color: var(--text-secondary);
            margin-top: 0.25rem;
            font-variant-numeric: tabular-nums;
        }
        .frequency-display.muted {
            color: var(--text-muted);
            font-style: italic;
        }

        @media (min-width: 768px) {
            .note-name { font-size: 5.5rem; }
        }
        @media (max-width: 360px) {
            .note-name { font-size: 3.5rem; }
        }
    `;

    constructor() {
        super();
        this.note = null;
        this.isActive = false;
    }

    render() {
        const n = this.note;
        const active = this.isActive && n;

        if (active) {
            return html`
                <div class="note-name-row">
                    <span class="accidental-indicator left">
                        ${n.isFlat ? html`<span class="indicator flat">\u266D</span>` : nothing}
                    </span>
                    <span class="note-name">${n.name}</span>
                    <span class="note-octave">${n.octave}</span>
                    <span class="accidental-indicator right">
                        ${n.isSharp ? html`<span class="indicator sharp">\u266F</span>` : nothing}
                        ${n.isInTune ? html`<span class="indicator intune">\u2713</span>` : nothing}
                    </span>
                </div>
                <div class="frequency-display">${n.detectedFreq.toFixed(1)} Hz</div>
            `;
        }

        return html`
            <div class="note-name-row">
                <span class="note-name muted">\u2014</span>
            </div>
            <div class="frequency-display muted">Waiting for signal...</div>
        `;
    }
}

customElements.define('note-display', NoteDisplay);
