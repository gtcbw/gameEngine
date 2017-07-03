using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound.Contracts;
using OpenTK.Audio.OpenAL;

namespace Sound
{
    public class Sound : IComplexSound
    {
        private int _source;

        public Sound(int source)
        {
            _source = source;
        }

        void IComplexSound.Pause()
        {
            int state;

            AL.GetSource(_source, ALGetSourcei.SourceState, out state);

            if ((ALSourceState)state == ALSourceState.Playing)
            {
                AL.SourcePause(_source);
            }
        }

        void IComplexSound.Continue()
        {
            int state;

            AL.GetSource(_source, ALGetSourcei.SourceState, out state);

            if ((ALSourceState)state == ALSourceState.Paused)
            {
                AL.SourcePlay(_source);
            }
        }

        void IComplexSound.Delete()
        {
            AL.DeleteSource(_source);
        }

        void ISound.Stop()
        {
            int state;
            AL.GetSource(_source, ALGetSourcei.SourceState, out state);

            if ((ALSourceState)state == ALSourceState.Playing)
            {
                AL.SourceStop(_source);
            }
        }

        void IComplexSound.SetVolume(float volume)
        {
            AL.Source(_source, ALSourcef.Gain, volume);
        }

        void IComplexSound.SetSpeed(float speedFactor)
        {
            AL.Source(_source, ALSourcef.Pitch, speedFactor);
        }

        void ISound.SetPosition(float x, float y, float z)
        {
            AL.Source(_source, ALSource3f.Position, x, y, z);
        }

        void ISound.Play()
        {
            AL.SourcePlay(_source);
        }
    }
}
