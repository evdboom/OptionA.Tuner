namespace OptionA.Tuner.Decoder.Opus
{
    [Flags]
    public enum DecoderMode
    {
        Silk = 1,
        Celt = 2,
        Hybrid = Silk | Celt
    }
}
