using Microsoft.AspNetCore.Components;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace OptionA.Tuner.Canvas
{
    [SupportedOSPlatform("browser")]
    public partial class Canvas
    {
        [Parameter]
        public byte[]? Frequencies { get; set; }

        //[JSImport("initialize", "Canvas")]
        //internal static partial Task Initialize();
        [JSImport("draw", "Canvas")]
        internal static partial void Draw(string canvasId, byte[] values);

        private readonly string _canvasId = "frequency-canvas";

        protected override async Task OnInitializedAsync()
        {
            await JSHost.ImportAsync("Canvas", "../Canvas/Canvas.razor.js");
            //await Initialize();            
        }

        protected override void OnParametersSet()
        {
            if (Frequencies is not null)
            {
                Draw(_canvasId, Frequencies);
            }
            
        }
    }
}
