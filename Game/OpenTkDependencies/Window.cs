using OpenTK;

namespace Game.OpenTkDependencies
{
    internal sealed class Window : GameWindow
    {
        internal Window(int resolutionX, int resolutionY)
            : base(resolutionX, resolutionY, new OpenTK.Graphics.GraphicsMode(new OpenTK.Graphics.ColorFormat(8,8,8,8), 24),"", GameWindowFlags.Fullscreen)
        {
            CursorVisible = false;
        }
    }
}
