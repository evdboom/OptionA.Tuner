export const getRecorder = async () => {
    const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
    const recorder = new MediaRecorder(stream);
    recorder.ondataavailable = onData;
    return recorder;
}

export const startRecorder = (recorder, sliceTime) => {
    recorder.start(sliceTime);
}

export const stopRecorder = (recorder) => {
    recorder.stop();
}

export const onData = async (event) => {    
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    var exports = await getAssemblyExports("OptionA.Tuner.dll"); 
    const buffer = await event.data.arrayBuffer();
    const stream = new Uint8Array(buffer);    
    console.log(event);
    exports.OptionA.Tuner.Pages.Index.ProcessSlice(stream);
}