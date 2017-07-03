using Graphics.Contracts;
using Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Initializer.Init();

            Graphics.Initializer.Initialize(new Configuration { Resolution = new Resolution { X = 800, Y = 600 } });

            Initializer.CleanUp();
        }
    }
}
