using Engine.Contracts.Input;
using OpenTK.Input;

namespace Game.OpenTkDependencies
{
    public sealed class MouseButtonEventProvider : IMouseButtonEventProvider
    {
        private MouseDevice _mouseDevice;
        private bool _leftButtonPressedLastTime;
        private bool _rightButtonPressedLastTime;

        public MouseButtonEventProvider(MouseDevice mouseDevice)
        {
            _mouseDevice = mouseDevice;
        }

        MouseButtonEvents IMouseButtonEventProvider.GetMouseEvents()
        {
            MouseButtonEvents mouseEvents = new MouseButtonEvents();

            mouseEvents.LeftButtonPressed = _mouseDevice[MouseButton.Left];
            mouseEvents.RightButtonPressed = _mouseDevice[MouseButton.Right];

            if (_leftButtonPressedLastTime && !mouseEvents.LeftButtonPressed)
                mouseEvents.LeftButtonReleased = true;

            if (_rightButtonPressedLastTime && !mouseEvents.RightButtonPressed)
                mouseEvents.RightButtonReleased = true;

            _leftButtonPressedLastTime = mouseEvents.LeftButtonPressed;
            _rightButtonPressedLastTime = mouseEvents.RightButtonPressed;

            mouseEvents.WheelDelta = _mouseDevice.WheelDelta;

            return mouseEvents;
        }
    }
}
