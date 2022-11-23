using System.Xml;

namespace OptionA.Tuner.Decoder.EBML
{
    public interface IEbmlParser
    {
        event EventHandler<EbmlDocument> DocumentReady;
        event EventHandler<string> ReadStepPerformed;
        event EventHandler<byte[]>? SimpleBlockReady;


        void SetSchema(XmlDocument schema);
        void ParseDocuments(byte[] stream);
    }
}
