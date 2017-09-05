using Engine.Contracts.Input;

namespace Engine.Framework
{
    public sealed class PressedKeyEncapsulator : IPressedKeyEncapsulator
    {
        private bool WasPressedLastTime;

        private Keys _key;
        private IPressedKeyDetector _pressedKeyDetector;

        public PressedKeyEncapsulator(Keys key, 
            IPressedKeyDetector pressedKeyDetector)
        {
            _key = key;
            _pressedKeyDetector = pressedKeyDetector;
        }

        bool IPressedKeyEncapsulator.KeyWasPressedOnce()
        {
            bool wasPressed = false;

            if (!WasPressedLastTime)
            {
                wasPressed = _pressedKeyDetector.IsKeyDown(_key);
            }
            WasPressedLastTime = _pressedKeyDetector.IsKeyDown(_key);

            return wasPressed;
        }
    }
}
