using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sound.Contracts
{
    public interface IComplexSound : ISound
    {
        void Pause();

        void Continue();

        void Delete();

        void SetVolume(float volume);

        void SetSpeed(float speedFactor);
    }
}
