namespace OptionA.Tuner.Decoder.Extensions
{
    public static class ByteExtensions
    {
        public static byte GetMostSignificantBits(this byte source, int bitcount)
        {
            return (byte)(source >> 8 - bitcount);
        }

        public static byte GetLeastSignificantBits(this byte source, int bitCount)
        {
            return GetMostSignificantBits((byte)(source << 8 - bitCount), bitCount);
        }
    }
}
