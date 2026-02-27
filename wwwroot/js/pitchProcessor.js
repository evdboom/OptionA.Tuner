// OptionA Tuner — AudioWorklet Processor
// Runs on a dedicated audio thread — buffers 128-sample frames into
// a 4096-sample ring buffer and posts the full buffer when ready.

class PitchProcessor extends AudioWorkletProcessor {
    constructor() {
        super();
        this._buffer = new Float32Array(4096);
        this._writeIndex = 0;
    }

    process(inputs, _outputs, _parameters) {
        const input = inputs[0];
        if (!input || input.length === 0) return true;

        const channel = input[0];
        if (!channel || channel.length === 0) return true;

        for (let i = 0; i < channel.length; i++) {
            this._buffer[this._writeIndex++] = channel[i];

            if (this._writeIndex >= this._buffer.length) {
                const copy = this._buffer.slice();
                this.port.postMessage(copy, [copy.buffer]);
                this._writeIndex = 0;
            }
        }

        return true;
    }
}

registerProcessor('pitch-processor', PitchProcessor);
