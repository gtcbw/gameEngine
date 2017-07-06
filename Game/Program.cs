using Game.OpenTkDependencies;
using Graphics.Contracts;
using Sound;
using OpenTK.Graphics.OpenGL;
using Engine.Contracts;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration c = new Configuration { Resolution = new Resolution { X = 800, Y = 600, AspectRatio = 16.0/9.0 } };

            Initializer.Init();
            
            Window window = new Window(c.Resolution.X, c.Resolution.Y);
            window.Resize += (sender, e) => { GL.Viewport(0, 0, c.Resolution.X, c.Resolution.Y); };

            IMouseController mouseController = new MouseController(window.Mouse, c.Resolution.X, c.Resolution.Y, c.Resolution.AspectRatio, c.InvertMouse);
            IPressedKeyDetector pressedKeyDetector = new PressedKeyDetector(window.Keyboard);

            Graphics.Initializer.Initialize();

            LoopCreator.BuildLoop()();

            Initializer.CleanUp();
        }
    }
}
