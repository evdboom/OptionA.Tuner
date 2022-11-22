using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace OptionA.Tuner.Pages
{
    [SupportedOSPlatform("browser")]
    public partial class Index
    {
        private JSObject? _recorder;

        private static event EventHandler<string>? OnData;

        [JSImport("getRecorder", "Index")]
        internal static partial Task<JSObject> GetRecorder();
        [JSImport("startRecorder", "Index")]
        internal static partial void StartRecorder(JSObject recorder, int timeslice);
        [JSImport("stopRecorder", "Index")]
        internal static partial void StopRecorder(JSObject recorder);
        [JSExport]
        internal static void ProcessSlice(byte[] slice)
        {
            var avg = slice.Average(b => b);
            OnData?.Invoke(null, $"{avg}");
        }

        private string _value = string.Empty;

        private bool _started = false;
        private void ClickMe()
        {
            if (_recorder is null)
            {
                return;
            }

            if (_started)
            {
                StopRecorder(_recorder);
            }
            else
            {
                StartRecorder(_recorder, 200);
            }

            _started = !_started;
        }

        protected override async Task OnInitializedAsync()
        {
            await JSHost.ImportAsync("Index", "../Pages/Index.razor.js");
            _recorder = await GetRecorder();
            OnData += Index_OnData;
        }

        private void Index_OnData(object? sender, string e)
        {
            _value = e;
            StateHasChanged();
        }
    }
}
