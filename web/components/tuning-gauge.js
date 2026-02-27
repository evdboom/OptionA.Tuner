// OptionA Tuner — Tuning Gauge Component
// Ported from Components/TuningGauge.razor

import { LitElement, html, css, svg, nothing } from '../vendor/lit.js';

// Pre-compute tick marks once (same maths as the Blazor version)
function generateTicks() {
    const ticks = [];
    const cx = 150, cy = 160, innerR = 108, outerR = 120, majorInnerR = 104;

    for (let cents = -50; cents <= 50; cents += 5) {
        const isMajor = cents % 10 === 0;
        const angleDeg = (cents * 90) / 50 - 90;
        const angleRad = (angleDeg * Math.PI) / 180;
        const r1 = isMajor ? majorInnerR : innerR;

        ticks.push({
            x1: cx + r1 * Math.cos(angleRad),
            y1: cy + r1 * Math.sin(angleRad),
            x2: cx + outerR * Math.cos(angleRad),
            y2: cy + outerR * Math.sin(angleRad),
            isMajor,
        });
    }
    return ticks;
}

const TICKS = generateTicks();

export class TuningGauge extends LitElement {
    static properties = {
        centsOffset: { type: Number },
        isActive: { type: Boolean },
    };

    static styles = css`
        :host { display: block; width: 100%; max-width: 340px; }

        .gauge-svg { width: 100%; height: auto; }

        .gauge-label {
            fill: var(--text-secondary);
            font-size: 11px;
            text-anchor: middle;
            font-family: inherit;
        }
        .gauge-label.center-label {
            font-weight: 700;
            fill: var(--color-intune);
            font-size: 13px;
        }

        .needle-group { transition: transform 0.15s ease-out; }
        :host(.inactive) .needle-group { opacity: 0.3; }

        .cents-display { text-align: center; margin-top: -0.5rem; }
        .cents-value {
            font-size: 0.8rem;
            font-weight: 500;
            font-variant-numeric: tabular-nums;
        }
        .cents-value.intune  { color: var(--color-intune); }
        .cents-value.warning { color: var(--color-warning); }
        .cents-value.flat,
        .cents-value.sharp   { color: var(--color-flat); }
        .cents-value.muted   { color: var(--text-muted); }

        @media (min-width: 768px) {
            :host { max-width: 400px; }
        }
    `;

    constructor() {
        super();
        this.centsOffset = 0;
        this.isActive = false;
    }

    get _needleAngle() {
        return this.isActive ? (this.centsOffset * 90) / 50 : 0;
    }

    get _centsColorClass() {
        const abs = Math.abs(this.centsOffset);
        if (abs <= 5) return 'intune';
        if (abs <= 15) return 'warning';
        return this.centsOffset < 0 ? 'flat' : 'sharp';
    }

    updated(changed) {
        if (changed.has('isActive')) {
            this.classList.toggle('inactive', !this.isActive);
        }
    }

    render() {
        const angle = this._needleAngle;

        return html`
            <svg viewBox="0 0 300 180" class="gauge-svg">
                <defs>
                    <linearGradient id="gaugeGradient" x1="0%" y1="0%" x2="100%" y2="0%">
                        <stop offset="0%"   stop-color="var(--color-flat)" />
                        <stop offset="30%"  stop-color="var(--color-warning)" />
                        <stop offset="45%"  stop-color="var(--color-intune)" />
                        <stop offset="55%"  stop-color="var(--color-intune)" />
                        <stop offset="70%"  stop-color="var(--color-warning)" />
                        <stop offset="100%" stop-color="var(--color-sharp)" />
                    </linearGradient>
                </defs>

                <!-- Gauge arc background -->
                <path d="M 30 160 A 120 120 0 0 1 270 160"
                      fill="none" stroke="var(--gauge-track)" stroke-width="12" stroke-linecap="round" />

                <!-- Colored gauge arc -->
                <path d="M 30 160 A 120 120 0 0 1 270 160"
                      fill="none" stroke="url(#gaugeGradient)" stroke-width="8"
                      stroke-linecap="round" opacity="0.7" />

                <!-- Tick marks -->
                ${TICKS.map(
                    (t) => svg`
                        <line x1="${t.x1}" y1="${t.y1}" x2="${t.x2}" y2="${t.y2}"
                              stroke="var(--text-secondary)"
                              stroke-width="${t.isMajor ? 2 : 1}"
                              opacity="${t.isMajor ? 0.8 : 0.4}" />`
                )}

                <!-- Tick labels -->
                <text x="25"  y="175" class="gauge-label">-50</text>
                <text x="150" y="30"  class="gauge-label center-label">0</text>
                <text x="268" y="175" class="gauge-label">+50</text>

                <!-- Center marker -->
                <line x1="150" y1="35" x2="150" y2="50"
                      stroke="var(--color-intune)" stroke-width="3" stroke-linecap="round" />

                <!-- Needle -->
                <g transform="rotate(${angle}, 150, 160)" class="needle-group">
                    <line x1="150" y1="160" x2="150" y2="48"
                          stroke="var(--needle-color)" stroke-width="3" stroke-linecap="round" />
                    <circle cx="150" cy="160" r="6" fill="var(--needle-color)" />
                </g>

                <!-- Center dot -->
                <circle cx="150" cy="160" r="3" fill="var(--bg-primary)" />
            </svg>

            <div class="cents-display">
                ${this.isActive
                    ? html`<span class="cents-value ${this._centsColorClass}">
                          ${this.centsOffset > 0 ? '+' : ''}${this.centsOffset.toFixed(1)} cents
                      </span>`
                    : html`<span class="cents-value muted">-- cents</span>`}
            </div>
        `;
    }
}

customElements.define('tuning-gauge', TuningGauge);
