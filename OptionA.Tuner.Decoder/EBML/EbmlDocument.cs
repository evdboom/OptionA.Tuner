using OptionA.Tuner.Decoder.EBML.Typed;

namespace OptionA.Tuner.Decoder.EBML
{
    public class EbmlDocument
    {
        public HeaderElement Header { get; init; }
        public List<IEbmlElement> Body { get; } = new();

        public EbmlDocument(HeaderElement header)
        {
            Header = header;            
        }
    }
}
