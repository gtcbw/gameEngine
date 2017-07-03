using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sound.Contracts
{
    public interface ISound
    {
        void Play();

        void Stop();

        void SetPosition(float x, float y, float z);
    }
}
