using System.Text;

namespace OptionA.Tuner.Decoder.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static byte[] ReadBytes(this BinaryReader reader, int length, bool bigEndian)
        {
            var bytes = reader.ReadBytes(length);
            if (bigEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        public static ulong ReadUnsignedInteger(this BinaryReader reader, int length)
        {
            if (length == 0)
            {
                return 0;
            }

            var bytes = ReadBytes(reader, length, true);
            var result = new byte[8];
            bytes.CopyTo(result, 0);
            return BitConverter.ToUInt64(result);
        }

        public static long ReadSignedInteger(this BinaryReader reader, int length)
        {
            if (length == 0)
            {
                return 0;
            }

            var bytes = ReadBytes(reader, length, true);            
            var result = new byte[8];
            bytes.CopyTo(result, 0);
            return BitConverter.ToInt64(result);
        }

        public static double ReadFloat(this BinaryReader reader, int length)
        {
            if (length == 0)
            {
                return 0;
            }

            var bytes = ReadBytes(reader, length, true);
            return length switch
            {
                sizeof(double) => BitConverter.ToDouble(bytes),
                sizeof(float) => BitConverter.ToSingle(bytes),
                _ => throw new InvalidOperationException("Incorrect length")
            };
        }

        public static string ReadString(this BinaryReader reader, int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            var bytes = ReadBytes(reader, length, false);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string ReadUtf8String(this BinaryReader reader, int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            var bytes = ReadBytes(reader, length, false);
            return Encoding.UTF8.GetString(bytes);
        }

        public static DateTime ReadDate(this BinaryReader reader, int length)
        {
            var baseDate = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (length == 0)
            {
                return baseDate;
            }

            var offset = ReadSignedInteger(reader, length);
            return baseDate.AddTicks(offset / 100);
        }
    }
}
