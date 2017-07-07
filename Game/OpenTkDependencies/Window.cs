using Graphics.Contracts;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Game.OpenTkDependencies
{
    internal sealed class Window : GameWindow, IBufferSwapper
    {
        internal Window(int resolutionX, int resolutionY)
            : base(resolutionX, resolutionY, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8,8,8,8), 24),"", GameWindowFlags.Fullscreen)
        {
            Resize += (sender, e) => { GL.Viewport(0, 0, resolutionX, resolutionY); };
            CursorVisible = false;
        }

        void IBufferSwapper.SwapBuffers()
        {
            SwapBuffers();
            ProcessEvents();
        }
    }
}
