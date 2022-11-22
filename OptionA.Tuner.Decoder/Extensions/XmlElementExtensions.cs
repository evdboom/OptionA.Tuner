using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OptionA.Tuner.Decoder.Extensions
{
    public static class XmlElementExtensions
    {
        private static readonly Dictionary<ulong, XmlElement> _foundElements = new();

        public static XmlElement? FindByTag(this XmlElement schema, ulong tag)
        {
            if (_foundElements.TryGetValue(tag, out XmlElement? element))
            {
                return element;
            }

            foreach(var child in schema.ChildNodes)
            {
                if (child is XmlElement xmlElement && xmlElement.GetAttribute("id") == $"0x{tag}")
                {
                    _foundElements[tag] = xmlElement;
                    return xmlElement;
                }                
            }

            return null;
        }
    }
}
