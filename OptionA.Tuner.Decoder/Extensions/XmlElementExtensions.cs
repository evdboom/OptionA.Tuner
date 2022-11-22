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
        public static XmlElement? FindByTag(this XmlElement schema, ulong tag)
        {
            foreach(var child in schema.ChildNodes)
            {
                if (child is XmlElement xmlElement && xmlElement.GetAttribute("id") == $"0x{tag}")
                {
                    return xmlElement;
                }                
            }

            return null;
        }
    }
}
