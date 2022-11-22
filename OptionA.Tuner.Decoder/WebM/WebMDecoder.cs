using OptionA.Tuner.Decoder.EBML;
using OptionA.Tuner.Decoder.Opus;
using System.Reflection;
using System.Xml;

namespace OptionA.Tuner.Decoder.WebM
{
    public class WebMDecoder : IWebMDecoder
    {
        private const string SchemaFile = "MatroskaSchema.xml";

        private readonly IOpusDecoder _opusDecoder;
        private readonly IEbmlParser _ebmlParser;
        private readonly XmlDocument _schema;        

        public WebMDecoder(IOpusDecoder opusDecoder, IEbmlParser ebmlParser)
        {
            _opusDecoder = opusDecoder;
            _ebmlParser = ebmlParser;

            _schema = new XmlDocument();
            _schema.LoadXml(MatroskaSchema.Schema);
            
        }

        public void Decode(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var ebmlStream = _ebmlParser.ParseDocuments(stream, _schema);
            };
        }
    }
}
