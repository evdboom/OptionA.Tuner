namespace OptionA.Tuner.Decoder.EBML.Typed
{
    public class MasterElement : EbmlElement<List<IEbmlElement>>
    {
        public MasterElement()
        {
            Value = new();
        }

        public void AddChild(IEbmlElement child) 
        {
            child.Parent = this;
            Value.Add(child);
        }
    }
}
