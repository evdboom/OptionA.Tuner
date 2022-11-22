using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionA.Tuner.Decoder.WebM
{
    public interface IWebMDecoder
    {
        void Decode(byte[] data);
    }
}
