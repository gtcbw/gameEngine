using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound.Contracts;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace Sound
{
    public class Listener : IEar
    {
        void IEar.SetPosition(float x, float y, float z)
        {
            AL.Listener(ALListener3f.Position, x, y, z);
        }
    }
}
