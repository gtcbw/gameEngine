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
        private readonly IKeyMapper _keyMapper;
        private readonly double _height = 100.8;
        private readonly double _metersPerSecond;

        private Position _position;
        private Ray _ray = new Ray();

        public PlayerPositionProvider(IPressedKeyDetector pressedKeyDetector,
            IHeightCalculator heightCalculator, 
            IFrameTimeProvider frameTimeProvider,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IVectorHelper vectorHelper,
            IKeyMapper keyMapper,
            double metersPerSecond,
            double startX,
            double startZ)
        {
            _pressedKeyDetector = pressedKeyDetector;
            _frameTimeProvider = frameTimeProvider;
            _heightCalculator = heightCalculator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _vectorHelper = vectorHelper;
            _keyMapper = keyMapper;
            _metersPerSecond = metersPerSecond;
            _position = new Position { X = startX, Z = startZ };
        }

        public void UpdatePosition()
        {
            ViewDirection direction = _playerViewDirectionProvider.GetViewDirection();

            Vector2D vectorXZ = _vectorHelper.ConvertDegreeToVector(direction.DegreeXZ);
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(direction.DegreeY);

            _ray.Direction = new Vector
            {
                X = vectorXZ.X * vectorY.X,
                Z = vectorXZ.Z * vectorY.X,
                Y = vectorY.Z
            };

            MovePosition(_position, vectorXZ);

            _position.Y = _heightCalculator.CalculateHeight(_position.X, _position.Z);

            _ray.StartPosition = new Position { X = _position.X, Y = _position.Y + _height, Z = _position.Z };
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
