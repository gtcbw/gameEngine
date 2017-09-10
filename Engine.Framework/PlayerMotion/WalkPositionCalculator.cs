using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class WalkPositionCalculator : IWalkPositionCalculator
    {
        private readonly IHeightCalculator _heightCalculator;
        private readonly IFrameTimeProvider _frameTimeProvider;
        private readonly IVectorHelper _vectorHelper;
        private readonly IKeyMapper _keyMapper;
        private readonly double _metersPerSecond;

        public WalkPositionCalculator(IHeightCalculator heightCalculator,
            IFrameTimeProvider frameTimeProvider,
            IVectorHelper vectorHelper,
            IKeyMapper keyMapper,
            double metersPerSecond)
        {
            _frameTimeProvider = frameTimeProvider;
            _heightCalculator = heightCalculator;
            _vectorHelper = vectorHelper;
            _keyMapper = keyMapper;
            _metersPerSecond = metersPerSecond;
        }

        Position IWalkPositionCalculator.CalculateNextPosition(Position currentPosition, Vector2D viewVectorXZ)
        {
            Position position = new Position { X = currentPosition.X, Z = currentPosition.Z };

            MovePosition(position, viewVectorXZ);

            position.Y = _heightCalculator.CalculateHeight(position.X, position.Z);

            return position;
        }

        private void MovePosition(Position position, Vector2D viewDirection)
        {
            MovementInstruction instruction = _keyMapper.GetMappedKeys();

            Vector2D forwardMovement = null;
            Vector2D sidewardMovement = null;

            if (instruction.WalkForward)
            {
                forwardMovement = viewDirection;
            }
            else if (instruction.WalkBackward)
            {
                forwardMovement = _vectorHelper.Rotate180Degree(viewDirection);
            }

            if (instruction.StrafeRight)
            {
                sidewardMovement = _vectorHelper.Rotate90Degree(viewDirection);
            }
            else if (instruction.StrafeLeft)
            {
                sidewardMovement = _vectorHelper.Rotate270Degree(viewDirection);
            }

            Vector2D movementVector = null;

            if (forwardMovement != null && sidewardMovement != null)
            {
                movementVector = new Vector2D
                {
                    X = (forwardMovement.X * 0.7070106) + (sidewardMovement.X * 0.7070106),
                    Z = (forwardMovement.Z * 0.7070106) + (sidewardMovement.Z * 0.7070106)
                };
            }
            else if (forwardMovement != null)
            {
                movementVector = forwardMovement;
            }
            else if (sidewardMovement != null)
            {
                movementVector = sidewardMovement;
            }

            if (movementVector == null)
                return;

            position.X += movementVector.X * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * _metersPerSecond;
            position.Z += movementVector.Z * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * _metersPerSecond;
        }
    }
}
