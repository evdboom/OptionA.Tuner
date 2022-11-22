namespace OptionA.Tuner.Decoder.Opus
{
    public record Bandwidth
    {
        public string Abbreviation { get; init; }
        public string Name { get; init; }
        public int AudioBandwidth { get; init; }
        public int SampleRate { get; init; }

        private Bandwidth(string abbreviation, string name, int audioBandwidth, int sampleRate)
        {
            Abbreviation = abbreviation;
            Name = name;
            AudioBandwidth = audioBandwidth;
            SampleRate = sampleRate;
        }

        public static readonly Dictionary<string, Bandwidth> Bandwidths = new()
        {
            { "NB", new Bandwidth("NB", "Narrowband", 4, 8) },
            { "MB", new Bandwidth("MB", "Medium band", 6, 12) },
            { "WB", new Bandwidth("WB", "Wideband", 8, 16) },
            { "SWB", new Bandwidth("SWB", "Super wideband", 12, 24) },
            { "FB", new Bandwidth("FB", "Fullband", 20, 48) },
        };
    }
}
