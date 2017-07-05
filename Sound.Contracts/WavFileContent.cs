using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sound.Contracts
{
    public sealed class WavFileContent
    {
        public byte[] ByteContent { set; get; }

        public int Channels { set;get; } 

        public int Bits {set;get; }

        public int Rate { set; get; } 
    }
}
