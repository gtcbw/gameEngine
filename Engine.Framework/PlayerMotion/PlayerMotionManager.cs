using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.Models;
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
    public sealed class PlayerMotionManager : IPlayerPositionProvider, IPlayerViewRayProvider, IPlayerViewDirectionProvider
    {
        private enum MotionModus
        {
            Walk = 0,
            Drive = 1
        }

        private IVectorHelper _vectorHelper;
        private readonly IWalkPositionCalculator _walkPositionCalculator;
        private readonly ICuboidWithWorldTester _cuboidWithWorldTester;
        private readonly IPressedKeyEncapsulator _enteredVehicleKey;
        private readonly IVehicleMotionCalculator _vehicleMotionCalculator;
        private readonly IMousePositionController _mousePositionController;
        private readonly IKeyMapper _keyMapper;
        private readonly IHeightCalculator _heightCalculator;
        private readonly IFrameTimeProvider _frameTimeProvider;
        private Position _position;
        private Ray _ray = new Ray();
        ViewDirection _direction;
        private double _height = 1.8;
        private double _playerSideLength = 0.6;
        private MotionModus _motionModus;
        private VehicleMotion _lastVehicleMotion;

        public PlayerMotionManager(IVectorHelper vectorHelper,
            IWalkPositionCalculator walkPositionCalculator,
            ICuboidWithWorldTester cuboidWithWorldTester,
            IPressedKeyEncapsulator enteredVehicleKey,
            IVehicleMotionCalculator vehicleMotionCalculator,
            IMousePositionController mousePositionController,
            IKeyMapper keyMapper,
            IHeightCalculator heightCalculator,
            IFrameTimeProvider frameTimeProvider,
            double startX,
            double startZ)
        {
            _vectorHelper = vectorHelper;
            _walkPositionCalculator = walkPositionCalculator;
            _cuboidWithWorldTester = cuboidWithWorldTester;
            _enteredVehicleKey = enteredVehicleKey;
            _vehicleMotionCalculator = vehicleMotionCalculator;
            _mousePositionController = mousePositionController;
            _keyMapper = keyMapper;
            _heightCalculator = heightCalculator;
            _frameTimeProvider = frameTimeProvider;
            _position = new Position { X = startX, Z = startZ };
        }

        Position IPlayerPositionProvider.GetPlayerPosition()
        {
            return _position;
        }

        Ray IPlayerViewRayProvider.GetPlayerViewRay()
        {
            return _ray;
        }

        ViewDirection IPlayerViewDirectionProvider.GetViewDirection()
        {
            return _direction;
        }

        public void CalculatePlayerMotion()
        {
            switch(_motionModus)
            {
                case MotionModus.Walk:
                    CalculateWalkPosition();
                    if (_enteredVehicleKey.KeyWasPressedOnce())
                        _motionModus = MotionModus.Drive;
                    return;
                case MotionModus.Drive:
                    CalculateDrivePosition();
                    if (_enteredVehicleKey.KeyWasPressedOnce())
                    {
                        _motionModus = MotionModus.Walk;
                        _lastVehicleMotion = null;
                    }
                    return;
            }
            
        }

        private void CalculateWalkPosition()
        {
            CalculateWalkViewDirection();
            Vector2D vectorXZ = _vectorHelper.ConvertDegreeToVector(_direction.DegreeXZ);
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(_direction.DegreeY);

            Position position = _walkPositionCalculator.CalculateNextPosition(_position, vectorXZ);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(position, _playerSideLength, _height))
            {
                _position = position;
            }

            _ray.Direction = new Vector
            {
                X = vectorXZ.X * vectorY.X,
                Z = vectorXZ.Z * vectorY.X,
                Y = vectorY.Z
            };
            _ray.StartPosition = new Position { X = _position.X, Y = _position.Y + _height, Z = _position.Z };
        }

        private double _viewDegreeXZ;
        private double _viewDegreeY;
        private double _maxDegreeY = 70;
        private void CalculateWalkViewDirection()
        {
            MousePositionDelta mousePositionDelta = _mousePositionController.MeasureMousePositionDelta();

           _viewDegreeXZ += mousePositionDelta.PositionDeltaX;

           _viewDegreeY += mousePositionDelta.PositionDeltaY;

            if (_viewDegreeXZ > 360.0)
                _viewDegreeXZ -= 360.0;
            else if (_viewDegreeXZ < 0.0)
                _viewDegreeXZ += 360.0;

            if (_viewDegreeY > _maxDegreeY)
                _viewDegreeY = _maxDegreeY;
            else if (_viewDegreeY < -_maxDegreeY)
                _viewDegreeY = -_maxDegreeY;

            _direction = new ViewDirection { DegreeXZ = _viewDegreeXZ, DegreeY = _viewDegreeY };
        }

        private double _relativeDriveDegreeXZ;
        private double _driveDegreeY;
        private double _driveMainDegreeXZ;
        private double _steeringWheelAngle;
        private double _drivingSpeed;
        private double _accelerationPerSecond = 15;
        private double _maxSpeed = 60;
        private void CalculateDriveViewDirection()
        {
            MousePositionDelta mousePositionDelta = _mousePositionController.MeasureMousePositionDelta();

            _relativeDriveDegreeXZ += mousePositionDelta.PositionDeltaX;

            _driveDegreeY += mousePositionDelta.PositionDeltaY;

            if (_relativeDriveDegreeXZ > 90.0)
                _relativeDriveDegreeXZ = 90.0;
            else if (_relativeDriveDegreeXZ < -90.0)
                _relativeDriveDegreeXZ = -90.0;

            if (_driveDegreeY > _maxDegreeY)
                _driveDegreeY = _maxDegreeY;
            else if (_driveDegreeY < 0)
                _driveDegreeY = 0;

            _direction = new ViewDirection { DegreeXZ = _driveMainDegreeXZ + _relativeDriveDegreeXZ, DegreeY = _driveDegreeY };
        }

        private void CalculateDrivePosition()
        {
            var keys = _keyMapper.GetMappedKeys();

            if (keys.StrafeLeft)
            {
                _steeringWheelAngle -= 15 * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (_steeringWheelAngle < -25)
                    _steeringWheelAngle = -25;
            }
            else if(keys.StrafeRight)
            {
                _steeringWheelAngle += 15 * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (_steeringWheelAngle > 25)
                    _steeringWheelAngle = 25;
            }

            if (keys.WalkForward)
            {
                _drivingSpeed += _accelerationPerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (_drivingSpeed > _maxSpeed)
                    _drivingSpeed = _maxSpeed;
            }
            else if (keys.WalkBackward)
            {
                _drivingSpeed -= _accelerationPerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (_drivingSpeed < -_maxSpeed / 2.0)
                    _drivingSpeed = -_maxSpeed / 2.0;
            }

            _driveMainDegreeXZ += _steeringWheelAngle * _drivingSpeed / 10.0 * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();

            CalculateDriveViewDirection();

            //if (_lastVehicleMotion == null)
            //{
            //    _lastVehicleMotion = new VehicleMotion { Position = _position, Speed = 0.0, SteeringWheelAngle = 0.0, MainDegreeXZ = 0.0 };
            //}

            //_lastVehicleMotion = _vehicleMotionCalculator.CalculateNextVehicleMotion(_lastVehicleMotion);
            Vector2D movementVector = _vectorHelper.ConvertDegreeToVector(_driveMainDegreeXZ);
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(_direction.DegreeY);

            Position position = new Position { X = _position.X, Z = _position.Z };
            position.X += movementVector.X * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * _drivingSpeed;
            position.Z += movementVector.Z * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * _drivingSpeed;
            position.Y = _heightCalculator.CalculateHeight(position.X, position.Z);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(position, _playerSideLength, _height))
            {
                _position = position;
            }

            Vector2D viewVectorXZ = _vectorHelper.ConvertDegreeToVector(_direction.DegreeXZ);

            _ray.Direction = new Vector
            {
                X = viewVectorXZ.X * vectorY.X,
                Z = viewVectorXZ.Z * vectorY.X,
                Y = vectorY.Z
            };
            _ray.StartPosition = new Position { X = _position.X, Y = _position.Y + _height, Z = _position.Z };
        }
    }
}
