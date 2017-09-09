using Engine.Contracts.Input;
using OpenTK.Input;
using System.Drawing;

namespace Game.OpenTkDependencies
{
    public sealed class MousePositionController : IMousePositionController
    {
        private MouseDevice _mouseDevice;
        private const int _fixPixelCountForMouseCenter = 200;
        private bool _invertMouse;
        private double _mouseSensitivity;

        public MousePositionController(MouseDevice mouseDevice,
            bool invertMouse, 
            double mouseSensitivity)
        {
            _mouseDevice = mouseDevice;
            _invertMouse = invertMouse;
            _mouseSensitivity = mouseSensitivity;
        }

        MousePositionDelta IMousePositionController.MeasureMousePositionDelta()
        {
            MousePositionDelta mousePositionDelta = new MousePositionDelta();

            Point cursor = System.Windows.Forms.Cursor.Position;

            mousePositionDelta.PositionDeltaX = cursor.X - _fixPixelCountForMouseCenter;
            mousePositionDelta.PositionDeltaY = _fixPixelCountForMouseCenter - cursor.Y;

            if (_invertMouse)
                mousePositionDelta.PositionDeltaY *= -1;

            mousePositionDelta.PositionDeltaX *= _mouseSensitivity;
            mousePositionDelta.PositionDeltaY *= _mouseSensitivity;

            System.Windows.Forms.Cursor.Position = new Point(_fixPixelCountForMouseCenter, _fixPixelCountForMouseCenter);

            return mousePositionDelta;
        }
    }
}
