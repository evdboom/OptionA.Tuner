using OptionA.Tuner.Decoder.EBML.Typed;

namespace OptionA.Tuner.Decoder.EBML
{
    public interface IEbmlElement
    {
        public MasterElement? Parent { get; set; }
        public ulong Tag { get; }
        public long StartsAt { get; }
        public ulong? Length { get; }
        public EbmlType Type { get; }
    }
}
