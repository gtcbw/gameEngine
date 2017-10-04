using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class ReboundCalculator : IReboundMotionCalculator
    {
        private IVectorHelper _vectorHelper;
        private IMousePositionController _mousePositionController;
        private IHeightCalculator _heightCalculator;
        private IFrameTimeProvider _frameTimeProvider;
        private double _maxSpeed = 150;
        private double _maxDegreeY = 70;

        public ReboundCalculator(IVectorHelper vectorHelper,
            IMousePositionController mousePositionController,
            IHeightCalculator heightCalculator,
            IFrameTimeProvider frameTimeProvider)
        {
            _vectorHelper = vectorHelper;
            _mousePositionController = mousePositionController;
            _heightCalculator = heightCalculator;
            _frameTimeProvider = frameTimeProvider;
        }

        ReboundMotion IReboundMotionCalculator.CalculateNextReboundMotion(ReboundMotion currentReboundMotion)
        {
            ReboundMotion reboundMotion = new ReboundMotion
            {
                MovementDegree = currentReboundMotion.MovementDegree,
                Speed = currentReboundMotion.Speed,
                MainViewDegreeXZ = currentReboundMotion.MainViewDegreeXZ,
                ViewDegreeY = currentReboundMotion.ViewDegreeY,
                RelativeViewDegreeXZ = currentReboundMotion.RelativeViewDegreeXZ
            };

            reboundMotion.Speed -= reboundMotion.Speed * 0.8 * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();

            if (reboundMotion.Speed < 0.1)
                reboundMotion.Speed = 0;

            reboundMotion.MainViewDegreeXZ += 1000 * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * reboundMotion.Speed / _maxSpeed;

            while (reboundMotion.MainViewDegreeXZ > 360)
                reboundMotion.MainViewDegreeXZ -= 360;

            Vector2D movementVector = _vectorHelper.ConvertDegreeToVector(reboundMotion.MovementDegree);

            reboundMotion.Position = new Position { X = currentReboundMotion.Position.X, Z = currentReboundMotion.Position.Z };
            reboundMotion.Position.X += movementVector.X * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * reboundMotion.Speed;
            reboundMotion.Position.Z += movementVector.Z * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * reboundMotion.Speed;
            reboundMotion.Position.Y = _heightCalculator.CalculateHeight(reboundMotion.Position.X, reboundMotion.Position.Z);

            MousePositionDelta mousePositionDelta = _mousePositionController.GetMousePositionDelta();

            reboundMotion.RelativeViewDegreeXZ += mousePositionDelta.PositionDeltaX;

            reboundMotion.ViewDegreeY += mousePositionDelta.PositionDeltaY;

            if (reboundMotion.RelativeViewDegreeXZ > 90.0)
                reboundMotion.RelativeViewDegreeXZ = 90.0;
            else if (reboundMotion.RelativeViewDegreeXZ < -90.0)
                reboundMotion.RelativeViewDegreeXZ = -90.0;

            if (reboundMotion.ViewDegreeY > _maxDegreeY)
                reboundMotion.ViewDegreeY = _maxDegreeY;
            else if (reboundMotion.ViewDegreeY < 0)
                reboundMotion.ViewDegreeY = 0;

            return reboundMotion;
        }
    }
}
