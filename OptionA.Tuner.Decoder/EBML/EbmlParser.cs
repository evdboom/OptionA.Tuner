using OptionA.Tuner.Decoder.EBML.Typed;
using OptionA.Tuner.Decoder.Extensions;
using System.Collections.Concurrent;
using System.Xml;

namespace OptionA.Tuner.Decoder.EBML
{
    public class EbmlParser : IEbmlParser
    {
        private const int SimpleBlockTag = 0xA3;

        public event EventHandler<EbmlDocument>? DocumentReady;
        public event EventHandler<string>? ReadStepPerformed;
        public event EventHandler<byte[]>? SimpleBlockReady;

        private XmlElement? _schema;
        private volatile bool _reading;
        private byte[]? _leftOver;
        private MasterElement? _currentElement;
        private EbmlDocument? _currentDocument;
        private IEbmlElement? _incomplete;

        private readonly ConcurrentQueue<byte[]> _streamQueues = new();


        private readonly Dictionary<string, EbmlType> _typeNames = new()
        {
            { "uinteger", EbmlType.UnsignedInteger },
            { "integer", EbmlType.SignedInteger },
            { "master", EbmlType.Master },
            { "binary", EbmlType.Binary },
            { "utf-8", EbmlType.Utf8 },
            { "float", EbmlType.Float },
            { "string", EbmlType.String },
            { "date", EbmlType.Date }
        };

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
            { 0xA3, new EbmlDefinition(0xA3, "SimpleBlock", @"\Segment\Cluster\SimpleBlock", EbmlType.Binary) }
        };

        public void SetSchema(XmlDocument schema)
        {
            var main = schema["EBMLSchema"];
            if (main is null)
            {
                throw new InvalidOperationException("provided schema is not an EBMLSchema");
            }
            _schema = main;
            ReadStepPerformed?.Invoke(this, "Schema set");
        }

        public void ParseDocuments(byte[] stream)
        {
            _streamQueues.Enqueue(stream);
            ReadStepPerformed?.Invoke(this, $"Stream of length {stream.Length} enqueued");

            if (!_reading && _schema is not null)
            {
                Start();
            }
        }

        private void Start()
        {
            ReadStepPerformed?.Invoke(this, "Begin parsing");
            if (!_streamQueues.TryDequeue(out byte[]? first))
            {
                ReadStepPerformed?.Invoke(this, "Nothing to parse");
                return;
            }

            _reading = true;
            var bytes = new List<byte[]>();
            if (_leftOver is not null)
            {
                ReadStepPerformed?.Invoke(this, "Found leftover parsing");
                bytes.Add(_leftOver);
                _leftOver = null;
            }
            bytes.Add(first);
            while (_streamQueues.TryDequeue(out byte[]? item))
            {
                bytes.Add(item);
            }

            var total = bytes
                .SelectMany(b => b)
                .ToArray();
            ReadStepPerformed?.Invoke(this, $"Begin processing stream of length {total.Length}");
            ProcessStream(total);
        }

        private void ProcessStream(byte[] bytes)
        {
            var reading = true;
            var incomplete = false;
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream))
            {
                while (reading)
                {
                    GetIncompleteValue(reader);
                    _incomplete = null;

                    EbmlDocument? newDocument = null;
                    var cannotReadValue = false;
                    var tag = ReadVariableSizeInteger(reader, false) ?? 0;
                    var length = ReadVariableSizeInteger(reader, true);
                    var dataStartsAt = reader.BaseStream.Position;
                    if (_definitions.TryGetValue(tag, out EbmlDefinition? definition))
                    {
                        ProcessDefinition(reader, definition, tag, length, dataStartsAt, ref _currentElement, _currentDocument, out newDocument, out cannotReadValue);
                    }
                    else if (_schema!.FindByTag(tag) is XmlElement xmlElement)
                    {
                        definition = BuildDefinition(tag, xmlElement);
                        _definitions[tag] = definition;
                        ProcessDefinition(reader, definition, tag, length, dataStartsAt, ref _currentElement, _currentDocument, out newDocument, out cannotReadValue);
                    }
                    else
                    {
                        var tagString = $"0x{tag:X}";
                        ReadStepPerformed?.Invoke(this, $"{tagString} not found in schema, {reader.BaseStream.Position} - {reader.BaseStream.Length}");
                    }
                    ReadStepPerformed?.Invoke(this, $"Found Definition: {definition?.Name}");
                    if (newDocument is not null)
                    {
                        if (_currentDocument is not null)
                        {
                            DocumentReady?.Invoke(this, _currentDocument);
                        }
                        _currentDocument = newDocument;
                    }
                    reading = !cannotReadValue && CanReadMore(reader);
                    incomplete = cannotReadValue || reader.BaseStream.Position < reader.BaseStream.Length;
                    if (cannotReadValue)
                    {
                        _incomplete = _currentElement is not null
                            ? _currentElement.Value!.Last()
                            : _currentDocument!.Body.Last();
                    }
                }

                var left = reader.BaseStream.Length - reader.BaseStream.Position;
                if (left > 0)
                {
                    _leftOver = reader.ReadBytes((int)left);
                }
            }
            if (!incomplete && _currentDocument is not null)
            {
                DocumentReady?.Invoke(this, _currentDocument);
                _currentDocument = null;
            }

            ReadStepPerformed?.Invoke(this, $"Performed parsing anyleftover?: {_leftOver is not null}");
            _reading = false;
            Start();
        }

        private void GetIncompleteValue(BinaryReader reader)
        {
            if (_incomplete is null)
            {
                return;
            }
            ReadStepPerformed?.Invoke(this, $"Incomple found to begin width");
            var readlength = (int)_incomplete.Length!;
            switch (_incomplete.Type)
            {
                case EbmlType.UnsignedInteger:
                    ((UnsignedIntegerElement)_incomplete).Value = reader.ReadUnsignedInteger(readlength);
                    break;
                case EbmlType.SignedInteger:
                    ((SignedIntegerElement)_incomplete).Value = reader.ReadSignedInteger(readlength);
                    break;
                case EbmlType.Date:
                    ((DateElement)_incomplete).Value = reader.ReadDate(readlength);
                    break;
                case EbmlType.Binary:
                    ((BinaryElement)_incomplete).Value = reader.ReadBytes(readlength);
                    break;
                case EbmlType.String:
                    ((StringElement)_incomplete).Value = reader.ReadString(readlength);
                    break;
                case EbmlType.Utf8:
                    ((StringElement)_incomplete).Value = reader.ReadUtf8String(readlength);
                    break;
                case EbmlType.Float:
                    ((FloatElement)_incomplete).Value = reader.ReadFloat(readlength);
                    break;
            }

            _incomplete = null;
        }

        private static bool CanReadMore(BinaryReader reader)
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                return false;
            }

            var result = true;
            var pos = reader.BaseStream.Position;
            ulong? length = null;
            if (!TryReadVariableSizeInteger(reader, false, out _))
            {
                result = false;
            }
            else if (!TryReadVariableSizeInteger(reader, true, out length))
            {
                result = false;
            }

            var canContinue = result &&
                (!length.HasValue || reader.BaseStream.Position + (long)length <= reader.BaseStream.Length);
            reader.BaseStream.Seek(pos, SeekOrigin.Begin);

            return canContinue;
        }

        private void ProcessDefinition(BinaryReader reader, EbmlDefinition definition, ulong tag, ulong? length, long dataStartsAt, ref MasterElement? currentElement, EbmlDocument? currentDocument, out EbmlDocument? newDocument, out bool cannotReadValue)
        {
            try
            {
                newDocument = null;
                var element = GetElement(reader, definition, tag, length, dataStartsAt, out cannotReadValue);
                if (definition.Type == EbmlType.Header)
                {
                    var header = (HeaderElement)element;
                    newDocument = new EbmlDocument(header);
                }
                else if (currentElement is not null)
                {
                    currentElement.AddChild(element, reader.BaseStream.Position, out MasterElement? currentOpen);
                    currentElement = currentOpen;
                }
                else
                {
                    currentDocument!.Body.Add(element);
                }

                if (element is MasterElement master)
                {
                    currentElement = master;
                }
                else if (element.Tag == SimpleBlockTag && !cannotReadValue)
                {
                    SimpleBlockReady?.Invoke(this, ((BinaryElement)element).Value!);
                }
            }
            catch (Exception e)
            {
                ReadStepPerformed?.Invoke(this, $"Exception during ProcessDefinition");
                ReadStepPerformed?.Invoke(this, e.Message);
                cannotReadValue = true;
                newDocument = null;
            }

        }

        private EbmlDefinition BuildDefinition(ulong tag, XmlElement xmlElement)
        {
            var name = xmlElement.GetAttribute("name");
            var path = xmlElement.GetAttribute("path");
            var type = xmlElement.GetAttribute("type");
            var defaultValue = xmlElement.GetAttribute("default");

            return new EbmlDefinition(tag, name, path, _typeNames[type], defaultValue);
        }

        private static IEbmlElement GetElement(BinaryReader reader, EbmlDefinition definition, ulong tag, ulong? length, long dataStartsAt, out bool cannotReadValue)
        {
            var readLength = (int)(length ?? 0);
            cannotReadValue = reader.BaseStream.Position + readLength > reader.BaseStream.Length;
            return definition.Type switch
            {
                EbmlType.Header => new HeaderElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Type = definition.Type,
                },
                EbmlType.String => new StringElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadString(readLength),
                    Type = definition.Type,
                },
                EbmlType.Utf8 => new StringElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadUtf8String(readLength),
                    Type = definition.Type,
                },
                EbmlType.Master => new MasterElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Type = definition.Type,
                },
                EbmlType.SignedInteger => new SignedIntegerElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadSignedInteger(readLength),
                    Type = definition.Type,
                },
                EbmlType.UnsignedInteger => new UnsignedIntegerElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadUnsignedInteger(readLength),
                    Type = definition.Type,
                },
                EbmlType.Binary => new BinaryElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadBytes(readLength),
                    Type = definition.Type,
                },
                EbmlType.Float => new FloatElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadFloat(readLength),
                    Type = definition.Type,
                },
                EbmlType.Date => new DateElement
                {
                    Tag = tag,
                    Length = length,
                    StartsAt = dataStartsAt,
                    Value = cannotReadValue ? default : reader.ReadDate(readLength),
                    Type = definition.Type,
                },
                _ => throw new NotImplementedException()
            };
        }

        private static ulong? ReadVariableSizeInteger(BinaryReader reader, bool justData)
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
                var first = byteList.FirstOrDefault();
                return justData && !IsValidLength(byteList, width)
                    ? null
                    : first;
            }

            var lenghtBytes = reader.ReadBytes(width - 1);
            byteList.AddRange(lenghtBytes);
            byteList.Reverse();

            var resultBytes = new byte[8];

            for (int i = 0; i < byteList.Count; i++)
            {
                resultBytes[i] = byteList[i];
            }

            var result = BitConverter.ToUInt64(resultBytes);
            return justData && !IsValidLength(byteList, width)
                ? null
                : result;
        }

        private static bool TryReadVariableSizeInteger(BinaryReader reader, bool justData, out ulong? result)
        {
            try
            {
                result = ReadVariableSizeInteger(reader, justData);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }

        }

        private static bool IsValidLength(List<byte> result, int width)
        {
            var zeros = string
                .Join(string.Empty, result
                    .Select(r => r.ToBinaryString()))
                .Where(c => c == '0')
                .Count();

            return zeros > width;
        }
    }
}
