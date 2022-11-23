using OptionA.Tuner.Decoder.EBML;
using OptionA.Tuner.Decoder.Opus;
using System.Xml;

namespace OptionA.Tuner.Decoder.WebM
{
    public class WebMDecoder : IWebMDecoder
    {
        public event EventHandler<string>? ReadStepPerformed;

        private readonly IOpusDecoder _opusDecoder;
        private readonly IEbmlParser _ebmlParser;
        private readonly XmlDocument _schema;

        public WebMDecoder(IOpusDecoder opusDecoder, IEbmlParser ebmlParser)
        {
            _opusDecoder = opusDecoder;
            _ebmlParser = ebmlParser;

            _schema = new XmlDocument();
            _schema.LoadXml(MatroskaSchema.Schema);
            _ebmlParser.SetSchema(_schema);
            _ebmlParser.DocumentReady += DocumentReady;
            _ebmlParser.ReadStepPerformed += OnReadStepPerformed;
            _ebmlParser.SimpleBlockReady += SimpleBlockReady;
        }

        private void SimpleBlockReady(object? sender, byte[] e)
        {
            _opusDecoder.DecodeStream(e);
        }

        private void DocumentReady(object? sender, EbmlDocument e)
        {
            ReadStepPerformed?.Invoke(this, $"{e.Body.Count}");
        }

        private void OnReadStepPerformed(object? sender, string e)
        {
            ReadStepPerformed?.Invoke(this, e);
        }

        public void Decode(byte[] data)
        {
            _ebmlParser.ParseDocuments(data);
        }
    }
}
