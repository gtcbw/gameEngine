using Engine.Contracts.Input;
using OpenTK.Input;

namespace Game.OpenTkDependencies
{
    public sealed class AbsoluteMousePositionProvider : IAbsoluteMousePositionProvider
    {
        private MouseDevice _mouseDevice;
        private const int _fixPixelCountForMouseCenter = 200;
        private double _resolutionX;
        private double _resolutionY;
        private double _aspectRatio;

        public AbsoluteMousePositionProvider(MouseDevice mouseDevice, int resolutionX, int resolutionY, double aspectRatio)
        {
            _mouseDevice = mouseDevice;
            _resolutionX = resolutionX;
            _resolutionY = resolutionY;
            _aspectRatio = aspectRatio;
        }

        MousePosition IAbsoluteMousePositionProvider.GetAbsolutePosition()
        {
            MousePosition mousePosition = new MousePosition();

            mousePosition.PositionX = (_mouseDevice.X) / (_resolutionX);
            mousePosition.PositionY = (_resolutionY - _mouseDevice.Y) / (_resolutionY);
            mousePosition.PositionX = (mousePosition.PositionX * _aspectRatio) - ((_aspectRatio - 1) / 2.0f);

            return mousePosition;
        }
    }
}
