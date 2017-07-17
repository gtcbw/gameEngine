using Engine.Contracts;
using Engine.Contracts.Input;
using Math.Contracts;
using World.Model;

namespace Engine.Framework
{
    public sealed class PlayerPositionProvider : IPlayerPositionProvider
    {
        private readonly IPressedKeyDetector _pressedKeyDetector;
        private readonly IHeightCalculator _heightCalculator;
        private readonly IFrameTimeProvider _frameTimeProvider;

        private Position _position = new Position();

        public PlayerPositionProvider(IPressedKeyDetector pressedKeyDetector,
            IHeightCalculator heightCalculator, 
            IFrameTimeProvider frameTimeProvider)
        {
            _pressedKeyDetector = pressedKeyDetector;
            _frameTimeProvider = frameTimeProvider;
            _heightCalculator = heightCalculator;
        }

        double IPlayerPositionProvider.GetHeight()
        {
            return 1.8;
        }

        Position IPlayerPositionProvider.GetPlayerPosition()
        {
            //_position.X
            return _position;
        }
    }
}
