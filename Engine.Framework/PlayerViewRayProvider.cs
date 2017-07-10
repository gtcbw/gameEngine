using Engine.Contracts;
using Math.Contracts;
using World.Model;

namespace Engine.Framework
{
    public sealed class PlayerViewRayProvider : IPlayerViewRayProvider
    {
        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IVectorHelper _vectorHelper;

        public PlayerViewRayProvider(IPlayerPositionProvider playerPositionProvider,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IVectorHelper vectorHelper)
        {
            _playerPositionProvider = playerPositionProvider;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _vectorHelper = vectorHelper;
        }

        Ray IPlayerViewRayProvider.GetPlayerViewRay()
        {
            Position position = _playerPositionProvider.GetPlayerPosition();

            Ray ray = new Ray();
            ray.StartPosition = new Position { X = position.X, Y = position.Y + _playerPositionProvider.GetHeight(), Z = position.Z };

            ViewDirection direction = _playerViewDirectionProvider.GetViewDirection();

            ray.Direction = _vectorHelper.CreateFromDegrees(direction.DegreeXZ, direction.DegreeY);
            ray.Direction = _vectorHelper.CalculateUnitLengthVector(ray.Direction);

            return ray;
        }
    }
}
