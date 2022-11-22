using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OptionA.Tuner.Decoder.EBML
{
    public interface IEbmlParser
    {
        EbmlStream ParseDocuments(Stream stream, XmlDocument schema);
    }
}
