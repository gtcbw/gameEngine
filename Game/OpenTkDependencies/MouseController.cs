using Engine.Contracts;
using OpenTK.Input;
using System.Drawing;

namespace Game.OpenTkDependencies
{
    public sealed class MouseController : IMouseController
    {
        private MouseDevice _mouseDevice;
        private bool _leftButtonPressedLastTime;
        private bool _rightButtonPressedLastTime;
        private const int _fixPixelCountForMouseCenter = 200;
        private bool _invertMouse;
        private double _resolutionX;
        private double _resolutionY;
        private double _aspectRatio;

        public MouseController(MouseDevice mouseDevice, int resolutionX, int resolutionY, double aspectRatio, bool invertMouse)
        {
            _mouseDevice = mouseDevice;
            _resolutionX = resolutionX;
            _resolutionY = resolutionY;
            _aspectRatio = aspectRatio;
            _invertMouse = invertMouse;
        }

        MouseEvents IMouseController.GetMouseEvents()
        {
            MouseEvents mouseEvents = new MouseEvents();

            mouseEvents.LeftButtonPressed = _mouseDevice[MouseButton.Left];
            mouseEvents.RightButtonPressed = _mouseDevice[MouseButton.Right];

            if (_leftButtonPressedLastTime && !mouseEvents.LeftButtonPressed)
                mouseEvents.LeftButtonReleased = true;

            if (_rightButtonPressedLastTime && !mouseEvents.RightButtonPressed)
                mouseEvents.RightButtonReleased = true;

            _leftButtonPressedLastTime = mouseEvents.LeftButtonPressed;
            _rightButtonPressedLastTime = mouseEvents.RightButtonPressed;

            mouseEvents.PositionX = (_mouseDevice.X) / (_resolutionX);
            mouseEvents.PositionY = (_resolutionY - _mouseDevice.Y) / (_resolutionY);
            mouseEvents.PositionX = (mouseEvents.PositionX * _aspectRatio) - ((_aspectRatio - 1) / 2.0f);

            mouseEvents.PositionXDelta = System.Windows.Forms.Cursor.Position.X - _fixPixelCountForMouseCenter;
            mouseEvents.PositionYDelta = _fixPixelCountForMouseCenter - System.Windows.Forms.Cursor.Position.Y;

            if (_invertMouse)
                mouseEvents.PositionYDelta *= -1;

            mouseEvents.WheelDelta = _mouseDevice.WheelDelta;

            return mouseEvents;
        }

        void IMouseController.ResetPosition()
        {
            System.Windows.Forms.Cursor.Position = new Point(_fixPixelCountForMouseCenter, _fixPixelCountForMouseCenter);
        }
    }
}
