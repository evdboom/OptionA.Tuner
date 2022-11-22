using OptionA.Tuner.Decoder.Extensions;

namespace OptionA.Tuner.Decoder.Opus
{
    public class OpusDecoder : IOpusDecoder
    {
        public void DecodeStream(Stream stream)
        {

            //128 64 32 16 8 4 2 1
            using (var reader = new BinaryReader(stream, System.Text.Encoding.Default, true))
            {
                var toc = reader.ReadByte();
                var config = toc.GetMostSignificantBits(5);
                var configuration = PacketConfiguration.Configurations[config];

                var flaggedToc = (BitFlags)toc;
                var stereo = flaggedToc.HasFlag(BitFlags.Five);

                var type = (PacketType)toc.GetLeastSignificantBits(2);

                ReadPacket(type, reader);
            }
        }

        private void ReadPacket(PacketType type, BinaryReader reader)
        {
            var padding = 0;
            var variable = false;
            var frames = 1;

            switch (type)
            {
                case PacketType.TwoEqualFrames:
                    frames = 2;
                    break;
                case PacketType.TwoDifferentFrames:
                    frames = 2;
                    variable = true;
                    break;
                case PacketType.ArbitraryNumberOfFrames:
                    GetFrameSpecifics(reader, out frames, out padding, out variable);
                    break;
            }

            var length = GetLengths(reader, frames, variable, padding);
            if (length.FirstOrDefault() == 0)
            {
                // no frame
                return;
            }
        }

        private void GetFrameSpecifics(BinaryReader reader, out int frames, out int padding, out bool variable)
        {
            var framesByte = reader.ReadByte();
            frames = framesByte.GetLeastSignificantBits(6);
            var flaggedFrames = (BitFlags)framesByte;

            var padded = flaggedFrames.HasFlag(BitFlags.One);
            variable = flaggedFrames.HasFlag(BitFlags.Zero);

            padding = padded
                ? GetPadding(reader)
                : 0;
        }

        private int GetPadding(BinaryReader reader)
        {
            var result = 0;
            byte padding;
            do
            {
                padding = reader.ReadByte();
                result += padding == byte.MaxValue
                    ? 254
                    : padding;
            }
            while (padding == byte.MaxValue);

            return result;
        }

        private IEnumerable<int> GetLengths(BinaryReader reader, int frameCount, bool variable, int padding)
        {
            var remaining = reader.BaseStream.Length - reader.BaseStream.Position - padding;
            if (!variable)
            {
                var length = (int)(remaining / frameCount);
                for (int i = 0; i < frameCount; i++)
                {
                    yield return length;
                }

                yield break;
            }

            for (int i = 0; i < frameCount - 1; i++)
            {
                var length = GetVariableLength(reader);
                remaining -= length;
                yield return length;
            }

            yield return (int)remaining;
        }

        private int GetVariableLength(BinaryReader reader)
        {
            var first = reader.ReadByte();
            if (first <= 251)
            {
                return first;
            }

            var second = reader.ReadByte();
            return second * 4 + first;
        }
    }
}
