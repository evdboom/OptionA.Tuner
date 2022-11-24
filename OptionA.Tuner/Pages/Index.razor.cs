using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace OptionA.Tuner.Pages
{
    [SupportedOSPlatform("browser")]
    public partial class Index
    {
        private static event EventHandler<(double SampleRate, byte[] Frequencies)>? DataReceived;

        [JSImport("initialize", "Index")]
        internal static partial Task Initialize();
        [JSImport("startRecorder", "Index")]
        internal static partial Task StartRecorder(double sampleRate, int fftSize, int highestFrequency);
        [JSImport("stopRecorder", "Index")]
        internal static partial void StopRecorder();
        [JSExport]
        internal static void Process(double sampleRate, byte[] slice)
        {
            DataReceived?.Invoke(null, (sampleRate, slice));
        }

        private double _sampleRate = 16000;
        private int _fftSize = 8192;
        private int _highestFrequency = 0;

        private byte[]? _slice;

        private bool _started = false;
        private async Task ClickMe()
        {
            if (_started)
            {
                StopRecorder();
            }
            else
            {
                await StartRecorder(_sampleRate, _fftSize, _highestFrequency);
            }

            _started = !_started;
        }

        protected override async Task OnInitializedAsync()
        {
            await JSHost.ImportAsync("Index", "../Pages/Index.razor.js");
            await Initialize();
            DataReceived += OnData;
        }

        private void OnData(object? sender, (double sampleRate, byte[] frequencies) slice)
        {
            var length = slice.frequencies.Length;
            var highest = slice.sampleRate / 2;

            var frSlice = highest / length;

            _slice = slice.frequencies;
                //.Select((s, i) => new FrequencySlice
                //{
                //    Value = s,
                //    FrStart = i * frSlice,
                //    FrEnd = (i + 1) * frSlice,
                //})
                //.ToList();
            StateHasChanged();
        }
    }
}
