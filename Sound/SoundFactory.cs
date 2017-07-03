using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sound.Contracts;
using System.Threading;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using FrameworkContracts;


namespace Sound
{
    public class SoundFactory : ISoundFactory, IResourceCleaner, ISoundInterrupter
    {
        private IBufferLoader _bufferLoader;
        private List<BufferSources> _buffers;
        private float _maxDistance;
        private float _volume;

        public SoundFactory(IBufferLoader bufferLoader, float maxDistance, float volume)
        {
            _bufferLoader = bufferLoader;
            _maxDistance = maxDistance;
            _volume = volume;
            _buffers = new List<BufferSources>();
            //AL.DistanceModel(ALDistanceModel.LinearDistance);
        }

        ISound ISoundFactory.LoadSound(string fileName, bool listenerDependent, bool looped = false)
        {
            BufferSources soundBuffer = _buffers.Find(x=>x.Buffer.FileName.Equals(fileName));

            if (soundBuffer == null)
            {
                soundBuffer = new BufferSources();
                soundBuffer.Sounds = new List<IComplexSound>();
                soundBuffer.Buffer = _bufferLoader.LoadBuffer(fileName);
                _buffers.Add(soundBuffer);
            }

            int source = AL.GenSource();
            AL.Source(source, ALSourcei.Buffer, soundBuffer.Buffer.Id);
            IComplexSound sound = new Sound(source);
            soundBuffer.Sounds.Add(sound);

            //listener settings
            AL.Source(source, ALSourcef.MaxDistance, _maxDistance);
            AL.Source(source, ALSourcef.ReferenceDistance, 20f);

            if (listenerDependent)
                AL.Source(source, ALSourceb.SourceRelative, false);
            else
                AL.Source(source, ALSourceb.SourceRelative, true);

            if (looped)
                AL.Source(source, ALSourceb.Looping, true);

            sound.SetVolume(_volume);

            return sound;
        }

        void IResourceCleaner.Clear()
        {
            ClearBufferAndSources();
        }

        void ISoundFactory.DeleteSound(ISound sound)
        {
            BufferSources soundbuffer = _buffers.Find(x => x.Sounds.Any(y => y == sound));

            DeleteBuffer(soundbuffer);

            _buffers.Remove(soundbuffer);
        }

        private void ClearBufferAndSources()
        {
            foreach (BufferSources soundbuffer in _buffers)
            {
                DeleteBuffer(soundbuffer);
            }
            _buffers.Clear();
        }

        private static void DeleteBuffer(BufferSources soundbuffer)
        {
            foreach (IComplexSound sound in soundbuffer.Sounds)
            {
                sound.Stop();
                sound.Delete();
            }

            AL.DeleteBuffer(soundbuffer.Buffer.Id);
        }

        void ISoundInterrupter.Pause()
        {
            foreach (BufferSources soundbuffer in _buffers)
            {
                foreach (IComplexSound sound in soundbuffer.Sounds)
                {
                    sound.Pause();
                }
            }
        }

        void ISoundInterrupter.Continue()
        {
            foreach (BufferSources soundbuffer in _buffers)
            {
                foreach (IComplexSound sound in soundbuffer.Sounds)
                {
                    sound.Continue();
                }
            }
        }
    }
}
