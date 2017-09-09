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
            double startX,
            double startZ)
        {
            _vectorHelper = vectorHelper;
            _walkPositionCalculator = walkPositionCalculator;
            _cuboidWithWorldTester = cuboidWithWorldTester;
            _enteredVehicleKey = enteredVehicleKey;
            _vehicleMotionCalculator = vehicleMotionCalculator;
            _mousePositionController = mousePositionController;
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
            CalculateViewDirection();
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

        private void CalculateViewDirection()
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

        private void CalculateDrivePosition()
        {
            if (_lastVehicleMotion == null)
            {
                _lastVehicleMotion = new VehicleMotion { Position = _position, Speed = 0.0, SteeringWheelAngle = 0.0, MainDegreeXZ = 0.0 };
            }

            _lastVehicleMotion = _vehicleMotionCalculator.CalculateNextVehicleMotion(_lastVehicleMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(_lastVehicleMotion.Position, 1.2, _height))
            {
                _position = _lastVehicleMotion.Position;
            }
            Vector2D vectorXZ = _vectorHelper.ConvertDegreeToVector(_lastVehicleMotion.MainDegreeXZ);
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(0.0);

            _ray.Direction = new Vector
            {
                X = vectorXZ.X * vectorY.X,
                Z = vectorXZ.Z * vectorY.X,
                Y = vectorY.Z
            };
            _ray.StartPosition = new Position { X = _position.X, Y = _position.Y + _height, Z = _position.Z };
        }

        ViewDirection IPlayerViewDirectionProvider.GetViewDirection()
        {
            return _direction;
        }
    }
}
