using Engine.Contracts.Input;
using OpenTK.Input;
using System.Drawing;

namespace Game.OpenTkDependencies
{
    public sealed class MousePositionDeltaProvider : IMousePositionDeltaProvider
    {
        private MouseDevice _mouseDevice;
        private const int _fixPixelCountForMouseCenter = 200;
        private bool _invertMouse;

        public MousePositionDeltaProvider(MouseDevice mouseDevice, bool invertMouse)
        {
            _mouseDevice = mouseDevice;
            _invertMouse = invertMouse;
        }

        MousePositionDelta IMousePositionDeltaProvider.GetPositionDelta()
        {
            MousePositionDelta mousePositionDelta = new MousePositionDelta();

            Point cursor = System.Windows.Forms.Cursor.Position;

            mousePositionDelta.PositionDeltaX = cursor.X - _fixPixelCountForMouseCenter;
            mousePositionDelta.PositionDeltaY = _fixPixelCountForMouseCenter - cursor.Y;

            if (_invertMouse)
                mousePositionDelta.PositionDeltaY *= -1;

            System.Windows.Forms.Cursor.Position = new Point(_fixPixelCountForMouseCenter, _fixPixelCountForMouseCenter);

            return mousePositionDelta;
        }
    }
}
