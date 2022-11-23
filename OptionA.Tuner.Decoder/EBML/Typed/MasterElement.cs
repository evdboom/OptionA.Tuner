namespace OptionA.Tuner.Decoder.EBML.Typed
{
    public class MasterElement : EbmlElement<List<IEbmlElement>>
    {
        public MasterElement()
        {
            Value = new();
        }

        public void AddChild(IEbmlElement child, long position, out MasterElement? currentOpenElement) 
        {
            child.Parent = this;
            Value!.Add(child);

            var ended = GetHighestEnded(this, position);
            currentOpenElement = ended is not null
                ? ended.Parent
                : this;


        }

        private MasterElement? GetHighestEnded(MasterElement? element, long position) 
        {
            if (element is null)
            {
                return null;
            }

            var endedParent = GetHighestEnded(element.Parent, position);
            if (endedParent is not null)
            {
                return endedParent;
            }
            else if (element.Length.HasValue && (element.StartsAt + (long)element.Length) <= position)
            {
                return element;
            }
            else
            {
                return null;
            }
        }
    }
}
