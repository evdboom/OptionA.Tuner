namespace OptionA.Tuner.Decoder.Opus
{
    public enum PacketType
    {
        OneFrame = 0,
        TwoEqualFrames = 1,
        TwoDifferentFrames = 2,
        ArbitraryNumberOfFrames = 3
    }
}
