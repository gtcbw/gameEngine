using Graphics.Contracts;
using Sound;
using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration c = new Configuration
            {
                //Resolution = new Resolution { X = 1366, Y = 768, AspectRatio = 1366.0 / 768.0 },
               Resolution = new Resolution { X = 1920, Y = 1080, AspectRatio = 1920.0 / 1080.0 },
               //Resolution = new Resolution { X = 800, Y = 600, AspectRatio = 800.0 / 600.0 },
                MouseSensitivity = 0.1
            };

            Initializer.Init();
            
            Action loop = LoopCreator.BuildLoop(c);

            Graphics.Initializer.Initialize();

            loop();

            Initializer.CleanUp();
        }
    }
}
