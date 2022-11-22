namespace OptionA.Tuner.Decoder.EBML
{
    public interface IEbmlElement
    {
        public IEbmlElement? Parent { get; set; }
        public ulong Tag { get; }
    }
}
