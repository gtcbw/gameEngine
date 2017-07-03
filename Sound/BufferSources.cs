using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound.Contracts;

namespace Sound
{
    public class BufferSources
    {
        public SoundBuffer Buffer { set; get; }

        public List<IComplexSound> Sounds { set; get; }
    }
}
