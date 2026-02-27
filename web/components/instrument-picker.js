// OptionA Tuner — Instrument Picker Component
// Ported from Components/InstrumentPicker.razor

import { LitElement, html, css, nothing } from '../vendor/lit.js';
import { getGrouped } from '../js/instruments.js';

export class InstrumentPicker extends LitElement {
    static properties = {
        selectedName: { type: String, attribute: 'selected-name' },
        _isOpen: { state: true },
        _searchText: { state: true },
    };

    static styles = css`
        :host {
            display: block;
            position: relative;
            min-width: 160px;
        }

        .toggle {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 0.5rem;
            width: 100%;
            background: var(--bg-card);
            color: var(--text-primary);
            border: 1px solid var(--border-color);
            border-radius: var(--radius-sm);
            padding: 0.4rem 0.6rem;
            font-size: 0.85rem;
            cursor: pointer;
            outline: none;
            transition: border-color var(--transition);
            text-align: left;
            white-space: nowrap;
            font-family: inherit;
        }

        .toggle:hover, :host(.open) .toggle {
            border-color: var(--color-accent);
        }

        .chevron {
            font-size: 0.7rem;
            color: var(--text-secondary);
            transition: transform var(--transition);
        }
        :host(.open) .chevron { transform: rotate(180deg); }

        .dropdown {
            position: absolute;
            top: calc(100% + 4px);
            left: 50%;
            transform: translateX(-50%);
            min-width: 220px;
            max-height: 320px;
            background: var(--bg-secondary);
            border: 1px solid var(--border-color);
            border-radius: var(--radius-sm);
            box-shadow: var(--shadow);
            z-index: 100;
            display: flex;
            flex-direction: column;
            overflow: hidden;
        }

        .search-wrap {
            position: relative;
            padding: 0.4rem;
            border-bottom: 1px solid var(--border-color);
            flex-shrink: 0;
        }

        .search {
            width: 100%;
            background: var(--bg-card);
            border: 1px solid var(--border-color);
            border-radius: 6px;
            padding: 0.35rem 1.8rem 0.35rem 0.5rem;
            font-size: 0.8rem;
            color: var(--text-primary);
            outline: none;
            transition: border-color var(--transition);
            font-family: inherit;
            box-sizing: border-box;
        }
        .search::placeholder { color: var(--text-muted); }
        .search:focus { border-color: var(--color-accent); }

        .search-clear {
            position: absolute;
            right: 0.6rem;
            top: 50%;
            transform: translateY(-50%);
            background: none;
            border: none;
            color: var(--text-muted);
            font-size: 0.7rem;
            cursor: pointer;
            padding: 0.2rem;
            line-height: 1;
        }
        .search-clear:hover { color: var(--text-primary); }

        .options {
            overflow-y: auto;
            flex: 1;
            scrollbar-width: thin;
            scrollbar-color: var(--border-color) transparent;
        }

        .group { padding-bottom: 0.2rem; }

        .group-header {
            font-size: 0.65rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.08em;
            color: var(--text-muted);
            padding: 0.5rem 0.6rem 0.2rem;
            position: sticky;
            top: 0;
            background: var(--bg-secondary);
        }

        .option {
            display: block;
            width: 100%;
            text-align: left;
            background: none;
            border: none;
            color: var(--text-primary);
            padding: 0.35rem 0.6rem 0.35rem 1rem;
            font-size: 0.8rem;
            cursor: pointer;
            transition: background var(--transition);
            font-family: inherit;
        }
        .option:hover { background: var(--bg-hover); }
        .option.selected { color: var(--color-accent); font-weight: 600; }

        .no-results {
            padding: 1rem;
            text-align: center;
            color: var(--text-muted);
            font-size: 0.8rem;
        }
    `;

    constructor() {
        super();
        this.selectedName = '';
        this._isOpen = false;
        this._searchText = '';
        this._grouped = getGrouped();

        // Close when clicking outside
        this._onDocClick = (e) => {
            if (this._isOpen && !this.contains(e.composedPath()[0])) {
                this._close();
            }
        };
    }

    connectedCallback() {
        super.connectedCallback();
        document.addEventListener('click', this._onDocClick, true);
    }

    disconnectedCallback() {
        super.disconnectedCallback();
        document.removeEventListener('click', this._onDocClick, true);
    }

    _toggle() {
        this._isOpen = !this._isOpen;
        if (this._isOpen) {
            this._searchText = '';
            this.classList.add('open');
            requestAnimationFrame(() => {
                const input = this.shadowRoot.querySelector('.search');
                if (input) input.focus();
            });
        } else {
            this.classList.remove('open');
        }
    }

    _close() {
        this._isOpen = false;
        this._searchText = '';
        this.classList.remove('open');
    }

    _onSearch(e) {
        this._searchText = e.target.value;
    }

    _onKeyDown(e) {
        if (e.key === 'Escape') this._close();
    }

    _select(inst) {
        this._close();
        this.dispatchEvent(new CustomEvent('instrument-changed', {
            detail: { name: inst.name },
            bubbles: true,
            composed: true,
        }));
    }

    render() {
        return html`
            <button class="toggle" @click=${this._toggle}>
                <span>${this.selectedName}</span>
                <span class="chevron">\u25BE</span>
            </button>
            ${this._isOpen ? this._renderDropdown() : nothing}
        `;
    }

    _renderDropdown() {
        const search = this._searchText.toLowerCase();
        let hasResults = false;

        const groups = [];
        for (const [category, list] of this._grouped) {
            const filtered = search
                ? list.filter(
                      (i) =>
                          i.name.toLowerCase().includes(search) ||
                          i.category.toLowerCase().includes(search)
                  )
                : list;
            if (filtered.length === 0) continue;
            hasResults = true;
            groups.push(html`
                <div class="group">
                    <div class="group-header">${category}</div>
                    ${filtered.map(
                        (inst) => html`
                            <button class="option ${inst.name === this.selectedName ? 'selected' : ''}"
                                    @click=${() => this._select(inst)}>
                                ${inst.name}
                            </button>`
                    )}
                </div>
            `);
        }

        return html`
            <div class="dropdown">
                <div class="search-wrap">
                    <input class="search" type="text" placeholder="Search instruments..."
                           .value=${this._searchText}
                           @input=${this._onSearch}
                           @keydown=${this._onKeyDown} />
                    ${this._searchText
                        ? html`<button class="search-clear" @click=${() => (this._searchText = '')}>\u2715</button>`
                        : nothing}
                </div>
                <div class="options">
                    ${hasResults ? groups : html`<div class="no-results">No instruments found</div>`}
                </div>
            </div>
        `;
    }
}

customElements.define('instrument-picker', InstrumentPicker);
