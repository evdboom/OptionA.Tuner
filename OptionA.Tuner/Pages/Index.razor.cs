﻿using Microsoft.AspNetCore.Components;
using OptionA.Tuner.Decoder.Opus;
using OptionA.Tuner.Decoder.WebM;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace OptionA.Tuner.Pages
{
    [SupportedOSPlatform("browser")]
    public partial class Index
    {
        private JSObject? _recorder;

        private static event EventHandler<byte[]>? OnData;

        [Inject]
        private IWebMDecoder Decoder { get; set; } = null!;

        [JSImport("getRecorder", "Index")]
        internal static partial Task<JSObject> GetRecorder();
        [JSImport("startRecorder", "Index")]
        internal static partial void StartRecorder(JSObject recorder, int timeslice);
        [JSImport("stopRecorder", "Index")]
        internal static partial void StopRecorder(JSObject recorder);
        [JSExport]
        internal static void ProcessSlice(byte[] slice)
        {            
            OnData?.Invoke(null, slice);
        }

        private List<string> _records = new();

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
            OnData += OnRecord;
            Decoder.ReadStepPerformed += GotMessage;
        }

        private void GotMessage(object? sender, string e)
        {
            _records.Add(e);
            StateHasChanged();
        }

        private void OnRecord(object? sender, byte[] slice)
        {
            Decoder.Decode(slice);
        }
    }
}
