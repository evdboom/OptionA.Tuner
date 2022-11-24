let dotNetClass;
let stream;
let interval;

export const initialize = async () => {
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    var exports = await getAssemblyExports("OptionA.Tuner.dll");
    dotNetClass = exports.OptionA.Tuner.Pages.Index;
}

export const startRecorder = async (sampleRate, fftSize, highestFrequency) => {
    if (!dotNetClass) {
        return ".Net not initialized yet!";
    }
    stream = await navigator.mediaDevices.getUserMedia({ audio: true });
    const audioCtx = new AudioContext({ sampleRate: sampleRate });
    const analyser = new AnalyserNode(audioCtx, { fftSize: fftSize });
    audioCtx
        .createMediaStreamSource(stream)
        .connect(analyser);
    const bufferSize = highestFrequency === 0
        ? analyser.frequencyBinCount
        : getHighest(sampleRate, analyser.frequencyBinCount, highestFrequency);
    interval = setInterval(() => {
        const data = new Float32Array(bufferSize);
        analyser.getFloatFrequencyData(data);
        draw("canvas1", data, bufferSize, false);
        const data2 = new Float32Array(fftSize)
        analyser.getFloatTimeDomainData(data2);
        draw("canvas2", data2, fftSize, true)
    }, 33)
}

const getHighest = (sampleRate, binCount, frequency) => {
    const max = sampleRate / 2;
    const slice = max / binCount;

    return frequency / slice;
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

const draw = (canvasId, values, bufferSize, timeMode) => {
    const canvas = document.getElementById(canvasId);
    const width = canvas.width;
    const height = canvas.height;
    const context = canvas.getContext("2d");
    context.clearRect(0, 0, width, height);

    const step = width / bufferSize;
    let x = 0;
    context.beginPath();
    let y = timeMode
        ? valueTime(values[0], height)
        : valueFrequency(values[0], height); 
    context.moveTo(x, y);
    for (let i = 1; i < bufferSize; i++) {
        x += step;        
        y = timeMode
            ? valueTime(values[i], height)
            : valueFrequency(values[i], height);
        context.lineTo(x, y);        
    }
    context.stroke();
}

const valueTime = (value, height) => {    
    return (height / 2) + (value * height);
}

const valueFrequency = (value, height) => {
    return value + 140;
}