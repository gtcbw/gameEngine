using Engine.Contracts.Input;
using System.Drawing;

namespace Game.OpenTkDependencies
{
    public sealed class MousePositionController : IMousePositionController
    {
        private const int _fixPixelCountForMouseCenter = 200;
        private int _minimumXCenter = 1000;
        private bool _invertMouse;
        private double _mouseSensitivity;
        private MousePositionDelta _mousePositionDelta;
        

        public MousePositionController(
            bool invertMouse, 
            double mouseSensitivity)
        {
            _invertMouse = invertMouse;
            _mouseSensitivity = mouseSensitivity;

            System.Windows.Forms.Cursor.Position = new Point(0, 0);
            Point cursor = System.Windows.Forms.Cursor.Position;
            _minimumXCenter = cursor.X + _fixPixelCountForMouseCenter;
        }

        MousePositionDelta IMousePositionController.GetMousePositionDelta()
        {
            return _mousePositionDelta;
        }

        public void MeasureMousePositionDelta()
        {
            _mousePositionDelta = new MousePositionDelta();

            Point cursor = System.Windows.Forms.Cursor.Position;

            _mousePositionDelta.PositionDeltaX = cursor.X - _minimumXCenter;
            _mousePositionDelta.PositionDeltaY = _fixPixelCountForMouseCenter - cursor.Y;

            if (_invertMouse)
                _mousePositionDelta.PositionDeltaY *= -1;

            _mousePositionDelta.PositionDeltaX *= _mouseSensitivity;
            _mousePositionDelta.PositionDeltaY *= _mouseSensitivity;

            System.Windows.Forms.Cursor.Position = new Point(_minimumXCenter, _fixPixelCountForMouseCenter);
        }
    }
}
