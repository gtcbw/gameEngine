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
        private readonly IMousePositionController _mousePositionController;
        private readonly IKeyMapper _keyMapper;
        private readonly double _metersPerSecond;
        private double _maxDegreeY = 70;

        public WalkPositionCalculator(IHeightCalculator heightCalculator,
            IFrameTimeProvider frameTimeProvider,
            IVectorHelper vectorHelper,
            IMousePositionController mousePositionController,
            IKeyMapper keyMapper,
            double metersPerSecond)
        {
            _frameTimeProvider = frameTimeProvider;
            _heightCalculator = heightCalculator;
            _vectorHelper = vectorHelper;
            _mousePositionController = mousePositionController;
            _keyMapper = keyMapper;
            _metersPerSecond = metersPerSecond;
        }

        WalkMotion IWalkPositionCalculator.CalculateNextPosition(WalkMotion lastWalkMotion)
        {
            WalkMotion walkMotion = new WalkMotion
            {
                Position = new Position { X = lastWalkMotion.Position.X, Z = lastWalkMotion.Position.Z },
                DegreeXZ = lastWalkMotion.DegreeXZ,
                DegreeY = lastWalkMotion.DegreeY
            };

            CalculateWalkViewDirection(walkMotion);

            walkMotion.VectorXZ = _vectorHelper.ConvertDegreeToVector(walkMotion.DegreeXZ);
            walkMotion.Motion = MovePosition(walkMotion.Position, walkMotion.VectorXZ);

            walkMotion.Position.Y = _heightCalculator.CalculateHeight(walkMotion.Position.X, walkMotion.Position.Z);

            return walkMotion;
        }
        private void CalculateWalkViewDirection(WalkMotion walkMotion)
        {
            MousePositionDelta mousePositionDelta = _mousePositionController.GetMousePositionDelta();

            walkMotion.DegreeXZ += mousePositionDelta.PositionDeltaX;

            walkMotion.DegreeY += mousePositionDelta.PositionDeltaY;

            if (walkMotion.DegreeXZ > 360.0)
                walkMotion.DegreeXZ -= 360.0;
            else if (walkMotion.DegreeXZ < 0.0)
                walkMotion.DegreeXZ += 360.0;

            if (walkMotion.DegreeY > _maxDegreeY)
                walkMotion.DegreeY = _maxDegreeY;
            else if (walkMotion.DegreeY < -_maxDegreeY)
                walkMotion.DegreeY = -_maxDegreeY;
        }

        private bool MovePosition(Position position, Vector2D viewDirection)
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
                return false;

            position.X += movementVector.X * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * _metersPerSecond;
            position.Z += movementVector.Z * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * _metersPerSecond;

            return true;
        }
    }
}
