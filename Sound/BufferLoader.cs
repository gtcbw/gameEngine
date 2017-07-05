using System;
using Sound.Contracts;
using OpenTK.Audio.OpenAL;

namespace Sound
{
    public sealed class BufferLoader : IBufferLoader
    {
        private IWavFileReader _wavFileReader;

        public BufferLoader(IWavFileReader wavFileReader)
        {
            _wavFileReader = wavFileReader;
        }

        SoundBuffer IBufferLoader.LoadBuffer(string fileName)
        {
            SoundBuffer soundBuffer = new SoundBuffer();
            soundBuffer.Id = AL.GenBuffer();
            soundBuffer.FileName = fileName;

            WavFileContent wavFileContent = _wavFileReader.LoadWave(fileName);
            AL.BufferData(soundBuffer.Id, GetSoundFormat(wavFileContent.Channels, wavFileContent.Bits), wavFileContent.ByteContent, wavFileContent.ByteContent.Length, wavFileContent.Rate);

            return soundBuffer;
        }

        private ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}
