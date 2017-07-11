using Graphics.Contracts;
using Sound;
using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration c = new Configuration { Resolution = new Resolution { X = 1920, Y = 1080, AspectRatio = 16.0/9.0 }, MouseSensitivity = 0.05 };

            Initializer.Init();
            
            Action loop = LoopCreator.BuildLoop(c);

            Graphics.Initializer.Initialize();

            loop();

            Initializer.CleanUp();
        }
    }
}
