// OptionA Tuner — Tuner App (top-level orchestrator)
// Ported from Pages/Tuner.razor

import { LitElement, html, css, nothing } from '../vendor/lit.js';
import { instruments } from '../js/instruments.js';
import { detectPitch } from '../js/pitch-detector.js';
import { mapNote } from '../js/note-mapper.js';
import * as audio from '../js/audio-service.js';
import * as storage from '../js/storage-service.js';

// Import child components (self-registering)
import './tuning-gauge.js';
import './note-display.js';
import './string-indicator.js';
import './instrument-picker.js';

const SMOOTHING_WINDOW = 5;

export class TunerApp extends LitElement {
    static properties = {
        _isSupported: { state: true },
        _isListening: { state: true },
        _currentNote: { state: true },
        _smoothedCents: { state: true },
        _selectedInstrument: { state: true },
        _referenceA4: { state: true },
    };

    static styles = css`
        :host {
            display: flex;
            flex-direction: column;
            min-height: 100vh;
            max-width: 480px;
            margin: 0 auto;
            padding: 1rem;
        }

        /* Header */
        header { text-align: center; padding-bottom: 1rem; }

        .header-brand {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
            margin-bottom: 1rem;
        }
        .header-logo { width: 32px; height: auto; }

        h1 {
            font-size: 1.4rem;
            font-weight: 600;
            color: var(--text-primary);
            margin: 0;
            letter-spacing: 0.02em;
        }

        .controls-row {
            display: flex;
            gap: 1rem;
            justify-content: center;
            flex-wrap: wrap;
        }
        .control-group {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 0.35rem;
        }
        .control-group label {
            font-size: 0.7rem;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            color: var(--text-secondary);
            font-weight: 500;
        }

        /* Pitch selector */
        .pitch-selector {
            display: flex;
            align-items: center;
            gap: 0.3rem;
            background: var(--bg-card);
            border: 1px solid var(--border-color);
            border-radius: var(--radius-sm);
            padding: 0.15rem;
        }
        .pitch-btn {
            width: 32px; height: 32px;
            border: none;
            background: transparent;
            color: var(--text-primary);
            font-size: 1.1rem;
            cursor: pointer;
            border-radius: 6px;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: background var(--transition);
            font-family: inherit;
        }
        .pitch-btn:hover:not(:disabled) { background: var(--bg-hover); }
        .pitch-btn:disabled { opacity: 0.3; cursor: not-allowed; }
        .pitch-value {
            font-size: 0.85rem;
            font-weight: 600;
            min-width: 55px;
            text-align: center;
            color: var(--text-primary);
        }

        /* Main area */
        main {
            flex: 1;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 0.5rem;
        }

        /* Buttons */
        .start-stop { padding: 1rem 0; display: flex; justify-content: center; }
        .btn-start, .btn-stop {
            border: none;
            border-radius: var(--radius);
            padding: 0.8rem 2rem;
            font-size: 1rem;
            font-weight: 600;
            cursor: pointer;
            transition: all var(--transition);
            display: flex;
            align-items: center;
            gap: 0.5rem;
            font-family: inherit;
        }
        .btn-start {
            background: linear-gradient(135deg, var(--color-accent), var(--color-accent-hover));
            color: white;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }
        .btn-start:hover { transform: translateY(-1px); box-shadow: 0 6px 20px rgba(102, 126, 234, 0.5); }
        .btn-start:active { transform: translateY(0); }
        .btn-icon { font-size: 1.2rem; }
        .btn-stop {
            background: var(--bg-card);
            color: var(--text-secondary);
            border: 1px solid var(--border-color);
        }
        .btn-stop:hover { background: var(--bg-hover); color: var(--text-primary); }

        /* Error */
        .error-message { text-align: center; padding: 2rem; color: var(--color-warning); }
        .error-message p { margin-bottom: 0.5rem; }

        /* Footer */
        footer {
            text-align: center;
            padding: 1rem 0;
            font-size: 0.75rem;
            color: var(--text-muted);
            letter-spacing: 0.05em;
        }

        @media (min-width: 768px) {
            :host  { padding: 2rem; }
            h1     { font-size: 1.6rem; }
        }
        @media (max-width: 360px) {
            .controls-row { gap: 0.5rem; }
        }
    `;

    constructor() {
        super();
        this._isSupported = audio.isSupported();
        this._isListening = false;
        this._currentNote = null;
        this._smoothedCents = 0;
        this._referenceA4 = 440;
        this._centsHistory = [];
        this._sampleRate = 44100;
        this._rafId = null;
        this._pendingNote = null;
        this._pendingCents = 0;
        this._dirty = false;

        // Restore preferences
        const inst = instruments.find((i) => i.name === (storage.getInstrument() ?? 'Cello')) ?? instruments[2];
        this._selectedInstrument = inst;

        const savedA4 = storage.getReferenceA4();
        if (savedA4 !== null && savedA4 >= 438 && savedA4 <= 446) {
            this._referenceA4 = savedA4;
        }
    }

    // ── Audio processing ──────────────────────────────────────

    _processBuffer = (samples, sampleRate) => {
        this._sampleRate = sampleRate;
        const freq = detectPitch(samples, sampleRate);

        if (freq !== null) {
            const note = mapNote(freq, this._referenceA4);
            this._pendingNote = note;

            this._centsHistory.push(note.centsOffset);
            if (this._centsHistory.length > SMOOTHING_WINDOW) this._centsHistory.shift();

            let sum = 0;
            for (let i = 0; i < this._centsHistory.length; i++) sum += this._centsHistory[i];
            this._pendingCents = sum / this._centsHistory.length;
        }

        // Mark dirty — actual DOM update happens on the next rAF
        this._dirty = true;
    };

    /**
     * Decouple audio processing from rendering by only flushing to Lit
     * at the display refresh rate.
     */
    _renderLoop = () => {
        if (!this._isListening) return;

        if (this._dirty) {
            this._dirty = false;
            this._currentNote = this._pendingNote;
            this._smoothedCents = this._pendingCents;
        }

        this._rafId = requestAnimationFrame(this._renderLoop);
    };

    async _startListening() {
        try {
            this._sampleRate = await audio.start(this._processBuffer);
            this._isListening = true;
            this._rafId = requestAnimationFrame(this._renderLoop);
        } catch (err) {
            console.error('Failed to start microphone:', err);
            this._isListening = false;
        }
    }

    _stopListening() {
        audio.stop();
        if (this._rafId != null) {
            cancelAnimationFrame(this._rafId);
            this._rafId = null;
        }
        this._isListening = false;
        this._currentNote = null;
        this._smoothedCents = 0;
        this._centsHistory = [];
        this._dirty = false;
    }

    disconnectedCallback() {
        super.disconnectedCallback();
        if (this._isListening) this._stopListening();
    }

    // ── Settings ──────────────────────────────────────────────

    _onInstrumentChanged(e) {
        const name = e.detail.name;
        const inst = instruments.find((i) => i.name === name);
        if (inst) {
            this._selectedInstrument = inst;
            storage.setInstrument(name);
        }
    }

    _incrementPitch() {
        if (this._referenceA4 < 446) {
            this._referenceA4++;
            storage.setReferenceA4(this._referenceA4);
        }
    }

    _decrementPitch() {
        if (this._referenceA4 > 438) {
            this._referenceA4--;
            storage.setReferenceA4(this._referenceA4);
        }
    }

    // ── Render ────────────────────────────────────────────────

    render() {
        return html`
            <header>
                <div class="header-brand">
                    <img src="logo.svg" alt="OptionA Tuner" class="header-logo" />
                    <h1>OptionA Tuner</h1>
                </div>
                <div class="controls-row">
                    <div class="control-group">
                        <label>Instrument</label>
                        <instrument-picker
                            selected-name=${this._selectedInstrument.name}
                            @instrument-changed=${this._onInstrumentChanged}>
                        </instrument-picker>
                    </div>
                    <div class="control-group">
                        <label>A4 Reference</label>
                        <div class="pitch-selector">
                            <button class="pitch-btn" @click=${this._decrementPitch}
                                    ?disabled=${this._referenceA4 <= 438}>\u2212</button>
                            <span class="pitch-value">${this._referenceA4} Hz</span>
                            <button class="pitch-btn" @click=${this._incrementPitch}
                                    ?disabled=${this._referenceA4 >= 446}>+</button>
                        </div>
                    </div>
                </div>
            </header>

            <main>
                ${!this._isSupported
                    ? html`
                          <div class="error-message">
                              <p>\u26A0 Your browser does not support microphone access.</p>
                              <p>Please use a modern browser (Chrome, Firefox, Edge, Safari).</p>
                          </div>`
                    : html`
                          <note-display
                              .note=${this._currentNote}
                              .isActive=${this._isListening}>
                          </note-display>

                          <tuning-gauge
                              .centsOffset=${this._smoothedCents}
                              .isActive=${this._isListening}>
                          </tuning-gauge>

                          <string-indicator
                              .instrument=${this._selectedInstrument}
                              .currentNote=${this._currentNote}
                              .isActive=${this._isListening}
                              .referenceA4=${this._referenceA4}>
                          </string-indicator>

                          <div class="start-stop">
                              ${!this._isListening
                                  ? html`<button class="btn-start" @click=${this._startListening}>
                                        <span class="btn-icon">\uD83C\uDFB5</span> Start Tuning
                                    </button>`
                                  : html`<button class="btn-stop" @click=${this._stopListening}>
                                        Stop
                                    </button>`}
                          </div>
                      `}
            </main>

            <footer>
                <span>No ads. No tracking. Just tuning.</span>
            </footer>
        `;
    }
}

customElements.define('tuner-app', TunerApp);
