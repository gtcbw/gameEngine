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
    public sealed class PlayerMotionManager : IPlayerPositionProvider, IPlayerViewRayProvider
    {
        private enum MotionModus
        {
            Walk = 0,
            Drive = 1
        }

        private IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private IVectorHelper _vectorHelper;
        private readonly IWalkPositionCalculator _walkPositionCalculator;
        private readonly ICuboidWithWorldTester _cuboidWithWorldTester;
        private readonly IPressedKeyEncapsulator _enteredVehicleKey;
        private readonly IVehicleMotionCalculator _vehicleMotionCalculator;
        private Position _position;
        private Ray _ray = new Ray();
        private double _height = 1.8;
        private double _playerSideLength = 0.6;
        private MotionModus _motionModus;
        private VehicleMotion _lastVehicleMotion;

        public PlayerMotionManager(IPlayerViewDirectionProvider playerViewDirectionProvider,
            IVectorHelper vectorHelper,
            IWalkPositionCalculator walkPositionCalculator,
            ICuboidWithWorldTester cuboidWithWorldTester,
            IPressedKeyEncapsulator enteredVehicleKey,
            IVehicleMotionCalculator vehicleMotionCalculator,
            double startX,
            double startZ)
        {
            _vectorHelper = vectorHelper;
            _walkPositionCalculator = walkPositionCalculator;
            _cuboidWithWorldTester = cuboidWithWorldTester;
            _enteredVehicleKey = enteredVehicleKey;
            _vehicleMotionCalculator = vehicleMotionCalculator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
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
            ViewDirection direction = _playerViewDirectionProvider.GetViewDirection();
            Vector2D vectorXZ = _vectorHelper.ConvertDegreeToVector(direction.DegreeXZ);
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(direction.DegreeY);

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
    }
}
