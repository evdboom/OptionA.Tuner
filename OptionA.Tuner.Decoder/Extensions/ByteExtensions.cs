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

        public static string ToBinaryString(this byte source) 
        {
            return Convert
                .ToString(source, 2)
                .PadLeft(8, '0');
        }
    }
}
