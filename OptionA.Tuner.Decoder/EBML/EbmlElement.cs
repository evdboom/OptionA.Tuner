using OptionA.Tuner.Decoder.EBML.Typed;

namespace OptionA.Tuner.Decoder.EBML
{
    public abstract class EbmlElement<Data> : IEbmlElement
    {
        public MasterElement? Parent { get; set; }
        public ulong Tag { get; set; }
        public ulong? Length { get; set; }
        public long StartsAt { get; set; }
        public Data? Value { get; set; }
        public EbmlType Type { get; set; }
    }
}
