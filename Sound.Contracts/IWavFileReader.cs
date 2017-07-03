using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sound.Contracts
{
    public interface IWavFileReader
    {
        WavFileContent LoadWave(string fileName);
    }
}
