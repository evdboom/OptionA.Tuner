using Microsoft.JSInterop;

namespace OptionA.Tuner.Services;

public class LocalStorageService(IJSRuntime js)
{
    private const string InstrumentKey = "tuner_instrument";
    private const string ReferenceA4Key = "tuner_referenceA4";

    public async Task<string?> GetInstrumentAsync()
    {
        return await GetItemAsync(InstrumentKey);
    }

    public async Task SetInstrumentAsync(string instrument)
    {
        await SetItemAsync(InstrumentKey, instrument);
    }

    public async Task<int?> GetReferenceA4Async()
    {
        var value = await GetItemAsync(ReferenceA4Key);
        return int.TryParse(value, out var result) ? result : null;
    }

    public async Task SetReferenceA4Async(int referenceA4)
    {
        await SetItemAsync(ReferenceA4Key, referenceA4.ToString());
    }

    private async Task<string?> GetItemAsync(string key)
    {
        return await js.InvokeAsync<string?>("localStorage.getItem", key);
    }

    private async Task SetItemAsync(string key, string value)
    {
        await js.InvokeVoidAsync("localStorage.setItem", key, value);
    }
}
