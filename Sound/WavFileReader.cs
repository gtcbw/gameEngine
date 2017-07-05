using Sound.Contracts;
using System.IO;

namespace Sound
{
    public sealed class WavFileReader : IWavFileReader
    {
        WavFileContent IWavFileReader.LoadWave(string fileName)
        {
            WavFileContent wavFileContent = new WavFileContent();

            Stream stream = File.Open(fileName, FileMode.Open);

            using (BinaryReader reader = new BinaryReader(stream))
            {
                string signature = new string(reader.ReadChars(4));
                int riff_chunck_size = reader.ReadInt32();
                string format = new string(reader.ReadChars(4));
                string format_signature = new string(reader.ReadChars(4));
                int formatchunksize = reader.ReadInt32();
                int audioformat = reader.ReadInt16();
                wavFileContent.Channels = reader.ReadInt16();
                wavFileContent.Rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                wavFileContent.Bits = reader.ReadInt16();
                string data_signature = new string(reader.ReadChars(4));
                int data_chunk_size = reader.ReadInt32();

                wavFileContent.ByteContent = reader.ReadBytes(data_chunk_size);
            }

            return wavFileContent;
        }
    }
}
