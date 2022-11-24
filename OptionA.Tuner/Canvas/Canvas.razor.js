let dotNetClass;

export const initialize = async () => {
    const { getAssemblyExports } = await globalThis.getDotnetRuntime(0);
    var exports = await getAssemblyExports("OptionA.Tuner.dll");
    dotNetClass = exports.OptionA.Tuner.Canvas.Canvas;
}

export const draw = (canvasId, values) => {
    const canvas = document.getElementById(canvasId);
    const context = canvas.getContext("2d");
    context.clearRect(0, 0, canvas.width, canvas.height);
    var step = canvas.width / values.length;

    context.beginPath();
    context.moveTo(0, canvas.height - values[0]);
    for (let i = 1; i < values.length; i++) {

        context.lineTo(i * step, canvas.height - values[i]);
    }
    context.closePath();
    context.stroke();
}