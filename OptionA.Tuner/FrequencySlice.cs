namespace OptionA.Tuner
{
    public record FrequencySlice
    {
        public byte Value { get; set; }
        public double FrStart { get; set; }
        public double FrEnd { get; set; }
        public string Rounded => $"{FrStart:0.##} - {FrEnd:0.##}: {Value}";
        public string? Name { get; set; }
    }
}
