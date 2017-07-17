using Engine.Contracts;
using Engine.Contracts.Input;
using Math.Contracts;
using World.Model;

namespace Engine.Framework
{
    public sealed class PlayerPositionProvider : IPlayerPositionProvider, IPlayerViewRayProvider
    {
        private readonly IPressedKeyDetector _pressedKeyDetector;
        private readonly IHeightCalculator _heightCalculator;
        private readonly IFrameTimeProvider _frameTimeProvider;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IVectorHelper _vectorHelper;
        private readonly double _height = 1.8;

        private Position _position = new Position();
        private Ray _ray = new Ray();

        public PlayerPositionProvider(IPressedKeyDetector pressedKeyDetector,
            IHeightCalculator heightCalculator, 
            IFrameTimeProvider frameTimeProvider,
             IPlayerViewDirectionProvider playerViewDirectionProvider,
            IVectorHelper vectorHelper)
        {
            _pressedKeyDetector = pressedKeyDetector;
            _frameTimeProvider = frameTimeProvider;
            _heightCalculator = heightCalculator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _vectorHelper = vectorHelper;
        }

        public void UpdatePosition()
        {
            Ray ray = new Ray();
            Position position = new Position();

            ViewDirection direction = _playerViewDirectionProvider.GetViewDirection();

            ray.Direction = _vectorHelper.CreateFromDegrees(direction.DegreeXZ, direction.DegreeY);
            ray.Direction = _vectorHelper.CalculateUnitLengthVector(ray.Direction);

            ray.StartPosition = new Position { X = position.X, Y = position.Y + _height, Z = position.Z };

            _position = position;
            _ray = ray;
        }

        Position IPlayerPositionProvider.GetPlayerPosition()
        {
            return _position;
        }

        Ray IPlayerViewRayProvider.GetPlayerViewRay()
        {
            return _ray;
        }
    }
}
