namespace OptionA.Tuner.Decoder.EBML.Typed
{
    public class HeaderElement : MasterElement
    {
        public UnsignedIntegerElement EBMLVersion => (UnsignedIntegerElement)Value!.Single(e => e.Tag == 0x4286);
        public UnsignedIntegerElement EBMLReadVersion => (UnsignedIntegerElement)Value!.Single(e => e.Tag == 0x42F7);
        public UnsignedIntegerElement EBMLMaxIDLength => (UnsignedIntegerElement)Value!.Single(e => e.Tag == 0x42F2);
        public UnsignedIntegerElement EBMLMaxSizeLength => (UnsignedIntegerElement)Value!.Single(e => e.Tag == 0x42F3);
        public StringElement DocType => (StringElement)Value!.Single(e => e.Tag == 0x4282);
        public UnsignedIntegerElement DocTypeVersion => (UnsignedIntegerElement)Value!.Single(e => e.Tag == 0x4287);
        public UnsignedIntegerElement DocTypeReadVersion => (UnsignedIntegerElement)Value!.Single(e => e.Tag == 0x4285);
        public MasterElement? DocTypeExtension => Value!.SingleOrDefault(e => e.Tag == 0x4281) as MasterElement;        
    }
}
