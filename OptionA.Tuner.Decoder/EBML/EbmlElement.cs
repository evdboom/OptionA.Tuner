namespace OptionA.Tuner.Decoder.EBML
{
    public abstract class EbmlElement<Data> : IEbmlElement
    {
        public IEbmlElement? Parent { get; set; }
        public ulong Tag { get; set; }
        public ulong? Length { get; set; }
        public Data? Value { get; set; }
    }
}
