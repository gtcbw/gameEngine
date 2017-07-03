using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sound.Contracts
{
    public interface ISoundInterrupter
    {
        void Pause();

        void Continue();
    }
}
