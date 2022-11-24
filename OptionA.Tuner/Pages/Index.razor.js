let dotNetClass;
let stream;
let interval;

export const initialize = async () => {
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    var exports = await getAssemblyExports("OptionA.Tuner.dll");
    dotNetClass = exports.OptionA.Tuner.Pages.Index;
}

export const startRecorder = async () => {
    if (!dotNetClass) {
        return ".Net not initialized yet!";
    }
    stream = await navigator.mediaDevices.getUserMedia({ audio: true });
    const audioCtx = new AudioContext({ sampleRate: 48000 });
    const analyser = new AnalyserNode(audioCtx, { fftSize: 4096 });
    audioCtx
        .createMediaStreamSource(stream)
        .connect(analyser);
    const bufferSize = analyser.frequencyBinCount;
    interval = setInterval(() => {
        const data = new Uint8Array(bufferSize);
        analyser.getByteFrequencyData(data);        
        draw("canvas1", data, bufferSize);
        const data2 = new Uint8Array(bufferSize * 2)
        analyser.getByteTimeDomainData(data2);
        draw("canvas2", data2, bufferSize * 2)
    }, 33)
}

export const stopRecorder = () => {
    if (!stream) {
        return;
    }
    clearInterval(interval);
    const tracks = stream.getTracks();
    for (const track of tracks) {
        if (track.readyState == "live") {
            track.stop();
        }
    }    
}

const draw = (canvasId, values, bufferSize) => {
    const canvas = document.getElementById(canvasId);
    const width = canvas.width;
    const height = canvas.height;
    const context = canvas.getContext("2d");
    context.clearRect(0, 0, width, height);
    const step = width / bufferSize;
    let x = step;
    context.beginPath();
    let value = values[0] / 128.0;
    let y = (value * height) / 2;
    context.moveTo(0, y);
    for (let i = 1; i < bufferSize; i++) {
        value = values[i] / 128.0;
        y = (value * height) / 2;
        context.lineTo(x, y);
        x += step;
    }
    context.stroke();
}