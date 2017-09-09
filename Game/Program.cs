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
                Resolution = new Resolution { X = 1366, Y = 768, AspectRatio = 1366.0 / 768.0 },
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
