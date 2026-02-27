using Microsoft.JSInterop;

namespace OptionA.Tuner.Services;

public sealed class AudioCaptureService : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private IJSObjectReference? _module;
    private DotNetObjectReference<AudioCaptureService>? _dotNetRef;

    public bool IsListening { get; private set; }
    public bool IsSupported { get; private set; } = true;
    public int SampleRate { get; private set; } = 44100;

    public event Action<float[]>? OnAudioDataReceived;

    public AudioCaptureService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private async Task EnsureModuleAsync()
    {
        _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/audioInterop.js");
    }

    public async Task<bool> CheckSupportedAsync()
    {
        await EnsureModuleAsync();
        IsSupported = await _module!.InvokeAsync<bool>("isSupported");
        return IsSupported;
    }

    public async Task StartAsync()
    {
        if (IsListening)
        {
            return;
        }

        await EnsureModuleAsync();
        _dotNetRef = DotNetObjectReference.Create(this);

        try
        {
            var sampleRate = await _module!.InvokeAsync<double>("startMicrophone", _dotNetRef);
            SampleRate = (int)sampleRate;
            IsListening = true;
        }
        catch (Exception ex)
        {
            _dotNetRef?.Dispose();
            _dotNetRef = null;
            Console.WriteLine($"Failed to start microphone: {ex.Message}");
            throw;
        }
    }

    public async Task StopAsync()
    {
        if (!IsListening)
        {
            return;
        }

        await EnsureModuleAsync();
        await _module!.InvokeVoidAsync("stopMicrophone");

        IsListening = false;
        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }

    [JSInvokable]
    public void ReceiveAudioData(byte[] bytes)
    {
        // Reconstruct float[] from the raw byte buffer sent by JS.
        // JS sends the backing ArrayBuffer of the Float32Array as a Uint8Array,
        // which Blazor transfers via its optimised byte[] fast path (no JSON).
        var floatCount = bytes.Length / sizeof(float);
        var buffer = new float[floatCount];
        Buffer.BlockCopy(bytes, 0, buffer, 0, bytes.Length);
        OnAudioDataReceived?.Invoke(buffer);
    }

    public async ValueTask DisposeAsync()
    {
        if (IsListening)
        {
            await StopAsync();
        }

        if (_module is not null)
        {
            await _module.DisposeAsync();
            _module = null;
        }

        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }
}
