using System.Collections.Generic;
using Sound.Contracts;

namespace Sound
{
    public sealed class BufferSources
    {
        public SoundBuffer Buffer { set; get; }

        public List<IComplexSound> Sounds { set; get; }
    }
}
