using Game.OpenTkDependencies;
using Graphics.Contracts;
using Sound;
using OpenTK.Graphics.OpenGL;
using Engine.Contracts;
using Engine.Framework;
using System;
using Engine.Contracts.Input;

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

            IMousePositionDeltaProvider mousePositionDeltaProvider = new MousePositionDeltaProvider(window.Mouse, c.InvertMouse);
            IMouseButtonEventProvider mouseButtonEventProvider = new MouseButtonEventProvider(window.Mouse);
            IPressedKeyDetector pressedKeyDetector = new PressedKeyDetector(window.Keyboard);
            IFrameTimeProvider timeProvider = new FrameTimeProvider();

            Graphics.Initializer.Initialize();

            Action loop = LoopCreator.BuildLoop();

            loop();

            Initializer.CleanUp();
        }
    }
}
