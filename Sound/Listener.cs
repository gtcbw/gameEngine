using Sound.Contracts;
using OpenTK.Audio.OpenAL;

namespace Sound
{
    public sealed class Listener : IEar
    {
        void IEar.SetPosition(float x, float y, float z)
        {
            AL.Listener(ALListener3f.Position, x, y, z);
        }
    }
}
