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
        private readonly double _height = 1.8;
        private readonly double _metersPerSecond = 2;

        private Position _position = new Position();
        private Ray _ray = new Ray();

        public PlayerPositionProvider(IPressedKeyDetector pressedKeyDetector,
            IHeightCalculator heightCalculator, 
            IFrameTimeProvider frameTimeProvider,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IVectorHelper vectorHelper,
            IKeyMapper keyMapper)
        {
            _pressedKeyDetector = pressedKeyDetector;
            _frameTimeProvider = frameTimeProvider;
            _heightCalculator = heightCalculator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _vectorHelper = vectorHelper;
            _keyMapper = keyMapper;
        }

        public void UpdatePosition()
        {
            Ray ray = new Ray();
            Position position = new Position();

            ViewDirection direction = _playerViewDirectionProvider.GetViewDirection();

            ray.Direction = _vectorHelper.CreateFromDegrees(direction.DegreeXZ, direction.DegreeY);

            MovePosition(position, ray.Direction);

            position.Y = _heightCalculator.CalculateHeight(position.X, position.Z);

            ray.StartPosition = new Position { X = position.X, Y = position.Y + _height, Z = position.Z };

            _position = position;
            _ray = ray;
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
