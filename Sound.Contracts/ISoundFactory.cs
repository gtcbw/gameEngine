using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sound.Contracts
{
    public interface ISoundFactory
    {
        ISound LoadSound(string fileName, bool listenerDependent, bool looped = false);

        void DeleteSound(ISound sound);
    }
}
