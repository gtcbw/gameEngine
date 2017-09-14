using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class ReboundCalculator : IReboundMotionCalculator
    {
        private IVectorHelper _vectorHelper;
        private IMousePositionController _mousePositionController;
        private IHeightCalculator _heightCalculator;
        private IFrameTimeProvider _frameTimeProvider;

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

            Vector2D movementVector = _vectorHelper.ConvertDegreeToVector(reboundMotion.MovementDegree);

            reboundMotion.Position = new Position { X = currentReboundMotion.Position.X, Z = currentReboundMotion.Position.Z };
            reboundMotion.Position.X += movementVector.X * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * reboundMotion.Speed;
            reboundMotion.Position.Z += movementVector.Z * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * reboundMotion.Speed;
            reboundMotion.Position.Y = _heightCalculator.CalculateHeight(reboundMotion.Position.X, reboundMotion.Position.Z);

            return reboundMotion;
        }
    }
}
