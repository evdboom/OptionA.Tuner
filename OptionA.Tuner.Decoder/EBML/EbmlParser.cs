using OptionA.Tuner.Decoder.EBML.Typed;
using OptionA.Tuner.Decoder.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace OptionA.Tuner.Decoder.EBML
{
    public class EbmlParser : IEbmlParser
    {
        private readonly Dictionary<ulong, EbmlDefinition> _definitions = new()
        {
            { 0x1A45DFA3, new EbmlDefinition(0x1A45DFA3, "EBML", @"\EBML", EbmlType.Header) },
            { 0x4286, new EbmlDefinition(0x4286, "EBMLVersion", @"\EBML\EBMLVersion", EbmlType.UnsignedInteger, 1) },
            { 0x42F7, new EbmlDefinition(0x42F7, "EBMLReadVersion", @"\EBML\EBMLReadVersion", EbmlType.UnsignedInteger, 1) },
            { 0x42F2, new EbmlDefinition(0x42F2, "EBMLMaxIDLength", @"\EBML\EBMLMaxIDLength", EbmlType.UnsignedInteger, 4) },
            { 0x42F3, new EbmlDefinition(0x42F3, "EBMLMaxSizeLength ", @"\EBML\EBMLMaxSizeLength", EbmlType.UnsignedInteger, 8) },
            { 0x4282, new EbmlDefinition(0x4282, "DocType ", @"\EBML\DocType", EbmlType.String) },
            { 0x4287, new EbmlDefinition(0x4287, "DocTypeVersion ", @"\EBML\DocTypeVersion", EbmlType.UnsignedInteger, 1) },
            { 0x4285, new EbmlDefinition(0x4285, "DocTypeReadVersion ", @"\EBML\DocTypeReadVersion", EbmlType.UnsignedInteger, 1) },
            { 0x4281, new EbmlDefinition(0x4281, "DocTypeExtension ", @"\EBML\DocTypeExtension", EbmlType.Master) },


        };

        public EbmlStream ParseDocuments(Stream stream, XmlDocument schema)
        {
            var main = schema["EBMLSchema"];
            if (main is null)
            {
                throw new InvalidOperationException("provided schema is nog an EBMLSchema");
            }
            int readBytes = 0;
            var reading = true;
            var result = new EbmlStream();
            using (var reader = new BinaryReader(stream))
            {
                MasterElement? currentElement = null;
                EbmlDocument? currentDocument = null;
                while (reading)
                {
                    var tag = TryReadVariableSizeInteger(reader, false, out ulong tagLong, out readBytes) ? tagLong : 0;
                    var length = TryReadVariableSizeInteger(reader, true, out ulong lengthLong, out readBytes) ? (ulong?)lengthLong : null;
                    var dataStartsAt = reader.BaseStream.Position;

                    if (_definitions.TryGetValue(tag, out EbmlDefinition? definition))
                    {
                        var element = GetElement(reader, definition, tag, length, dataStartsAt);
                    }
                    else if (main.FindByTag(tag) is XmlElement element)
                    {

                    }

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
                    else if (_definitions.TryGetValue(tag, out EbmlType type))
                    {
                        switch (type)
                        {
                            case EbmlType.String:
                                currentElement.AddChild(new StringElement
                                {
                                    Tag = tag,
                                    Length = length,
                                    Value = reader.ReadString((int)length, out readBytes)
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

        private IEbmlElement GetElement(BinaryReader reader, EbmlDefinition definition, ulong tag, ulong? length, long dataStartsAt, out int bytesRead)
        {
            bytesRead = 0;
            var readLength = (int)(length ?? 0);
            return definition.Type switch
            {
                EbmlType.Header => new HeaderElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt
                },
                EbmlType.String => new StringElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = reader.ReadString(readLength, out bytesRead)
                },
                EbmlType.Utf8 => new StringElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = reader.ReadUtf8String(readLength, out bytesRead)
                },
                EbmlType.Master => new MasterElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt
                },
                EbmlType.SignedInteger => new SignedIntegerElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = reader.ReadSignedInteger(readLength, out bytesRead)
                },
                EbmlType.UnsignedInteger => new UnsignedIntegerElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = reader.ReadUnsignedInteger(readLength, out bytesRead)
                },
                _ => throw new NotImplementedException()
            };
        }

        private static bool TryReadVariableSizeInteger(BinaryReader reader, bool justData, [MaybeNullWhen(false)] out ulong result, out int readBytes)
        {
            readBytes = 0;
            var byteList = new List<byte>();
            bool found;
            var width = 0;
            do
            {
                var currentByte = reader.ReadByte();
                readBytes++;
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
            readBytes += width - 1;
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
