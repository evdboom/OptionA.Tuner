namespace OptionA.Tuner.Decoder.EBML
{
    public record EbmlDefinition
    {
        public ulong Tag { get; }
        public string Name { get; }
        public string Path { get; }
        public EbmlType Type { get; }
        public object? DefaultValue { get; }

        public EbmlDefinition(ulong tag, string name, string path, EbmlType type) : this(tag, name, path, type, null)
        { }

        public EbmlDefinition(ulong tag, string name, string path, EbmlType type, object? defaultValue)
        {
            Tag = tag;
            Name = name;
            Path = path;
            Type = type;
            DefaultValue = defaultValue;
        }
    }
}
