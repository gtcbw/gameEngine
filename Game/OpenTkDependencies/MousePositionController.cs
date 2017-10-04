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
        private MousePositionDelta _mousePositionDelta;

        public MousePositionController(MouseDevice mouseDevice,
            bool invertMouse, 
            double mouseSensitivity)
        {
            _mouseDevice = mouseDevice;
            _invertMouse = invertMouse;
            _mouseSensitivity = mouseSensitivity;
        }

        MousePositionDelta IMousePositionController.GetMousePositionDelta()
        {
            return _mousePositionDelta;
        }

        public void MeasureMousePositionDelta()
        {
            _mousePositionDelta = new MousePositionDelta();

            Point cursor = System.Windows.Forms.Cursor.Position;

            _mousePositionDelta.PositionDeltaX = cursor.X - _fixPixelCountForMouseCenter;
            _mousePositionDelta.PositionDeltaY = _fixPixelCountForMouseCenter - cursor.Y;

            if (_invertMouse)
                _mousePositionDelta.PositionDeltaY *= -1;

            _mousePositionDelta.PositionDeltaX *= _mouseSensitivity;
            _mousePositionDelta.PositionDeltaY *= _mouseSensitivity;

            System.Windows.Forms.Cursor.Position = new Point(_fixPixelCountForMouseCenter, _fixPixelCountForMouseCenter);
        }
    }
}
