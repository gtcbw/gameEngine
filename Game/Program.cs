using Graphics.Contracts;
using Sound;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Initializer.Init();

            Graphics.Initializer.Initialize(new Configuration { Resolution = new Resolution { X = 800, Y = 600 } });

            LoopCreator.BuildLoop()();

            Initializer.CleanUp();
        }
    }
}
