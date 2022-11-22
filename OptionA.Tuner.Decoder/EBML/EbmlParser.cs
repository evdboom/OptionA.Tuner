using OptionA.Tuner.Decoder.EBML.Typed;
using OptionA.Tuner.Decoder.Extensions;
using System.Xml;

namespace OptionA.Tuner.Decoder.EBML
{
    public class EbmlParser : IEbmlParser
    {
        private readonly Dictionary<ulong, EbmlType> _headerTypes = new()
        {
            { 0x4286, EbmlType.UnsignedInteger },
            { 0x42F7, EbmlType.UnsignedInteger },
            { 0x42F2, EbmlType.UnsignedInteger },
            { 0x42F3, EbmlType.UnsignedInteger },
            { 0x4282, EbmlType.String },
            { 0x4287, EbmlType.UnsignedInteger },
            { 0x4285, EbmlType.UnsignedInteger },
            { 0x4281, EbmlType.Master },
        };

        public EbmlStream ParseDocuments(Stream stream, XmlDocument schema)
        {
            var main = schema["EBMLSchema"];
            if (main is null)
            {
                throw new InvalidOperationException("provided schema is nog an EBMLSchema");
            }

            var reading = true;
            var result = new EbmlStream();
            using (var reader = new BinaryReader(stream))
            {
                MasterElement? currentElement = null;
                EbmlDocument? currentDocument = null;
                while (reading)
                {
                    var tag = TryReadVariableSizeInteger(reader, false, out ulong? tagLong) ? tagLong.Value : 0;
                    var length = TryReadVariableSizeInteger(reader, true, out ulong? lengthLong) ? lengthLong : null;
                    if (tag == 0x1A45DFA3)
                    {
                        var header = new HeaderElement
                        {
                            Tag = tag,
                            Length = length
                        };
                        currentElement = header;
                        currentDocument = new EbmlDocument(header);
                        result.Documents.Add(currentDocument);
                    }
                    else if (main.FindByTag(tag) is XmlElement element)
                    {
                        
                    }
                    else if (_headerTypes.TryGetValue(tag, out EbmlType type))
                    {
                        switch(type) 
                        {
                            case EbmlType.String:
                                currentElement.AddChild(new StringElement
                                {
                                    Tag = tag,
                                    Length = length,
                                    Value = reader.ReadString((int)length)
                                });
                                break;
                            case EbmlType.UnsignedInteger:
                                currentElement.AddChild(new UnsignedIntegerElement
                                {
                                    Tag = tag,
                                    Length = length,
                                    Value = reader.ReadUnsignedInteger((int)length)
                                });
                                break;
                            case EbmlType.SignedInteger:
                                currentElement.AddChild(new SignedIntegerElement
                                {
                                    Tag = tag,
                                    Length = length,
                                    Value = reader.ReadSignedInteger((int)length)
                                });
                                break;                               
                        }
                    }
                }
            }


            return result;
        }

        private static bool TryReadVariableSizeInteger(BinaryReader reader, bool justData, out ulong? result)
        {
            var byteList = new List<byte>();
            bool found;
            var width = 0;
            do
            {
                var currentByte = reader.ReadByte();
                byteList.Add(currentByte);
                
                found = currentByte > 0;
                if (currentByte <= 1)
                {
                    width += 8;
                }
                else
                {
                    for (int i = 1; i < 8; i++)
                    {
                        width++;
                        var bit = currentByte.GetMostSignificantBits(i);
                        if (bit == 1)
                        {
                            break;
                        }
                    }
                }
            }
            while (!found);

            if (justData)
            {
                var shift = 8 - (width % 8);
                byteList[^1] = byteList[^1].GetLeastSignificantBits(shift);
            }

            if (width == 1)
            {
                result = byteList.FirstOrDefault();
                return justData
                    ? IsValidLength(result)
                    : true;
            }

            var lenghtBytes = reader.ReadBytes(width - 1);
            byteList.AddRange(lenghtBytes);
            byteList.Reverse();

            var resultBytes = new byte[8];

            for(int i = 0; i < byteList.Count; i++)
            {
                resultBytes[i] = byteList[i];
            }
            result = BitConverter.ToUInt64(resultBytes);
            return justData
                ? IsValidLength(result)
                : true;
        }

        private bool IsValidLength(byte[] result)
        {
            return false;
        }
    }
}
