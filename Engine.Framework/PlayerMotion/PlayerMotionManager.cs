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
            Drive = 1,
            Rebound = 2
        }

        private IVectorHelper _vectorHelper;
        private readonly IWalkPositionCalculator _walkPositionCalculator;
        private readonly ICuboidWithWorldTester _cuboidWithWorldTester;
        private readonly IPressedKeyEncapsulator _enteredVehicleKey;
        private readonly IVehicleMotionCalculator _vehicleMotionCalculator;
        private readonly IReboundMotionCalculator _reboundMotionCalculator;
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
        private WalkMotion _lastWalkMotion;
        private ReboundMotion _lastReboundMotion;

        public PlayerMotionManager(IVectorHelper vectorHelper,
            IWalkPositionCalculator walkPositionCalculator,
            ICuboidWithWorldTester cuboidWithWorldTester,
            IPressedKeyEncapsulator enteredVehicleKey,
            IVehicleMotionCalculator vehicleMotionCalculator,
            IReboundMotionCalculator reboundMotionCalculator,
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
            _reboundMotionCalculator = reboundMotionCalculator;
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
                    {
                        _motionModus = MotionModus.Drive;
                        _lastWalkMotion = null;
                    }
                    return;
                case MotionModus.Drive:
                    CalculateDrivePosition();
                    if (_enteredVehicleKey.KeyWasPressedOnce())
                    {
                        _motionModus = MotionModus.Walk;
                        _lastVehicleMotion = null;
                    }
                    return;
                case MotionModus.Rebound:
                    CalculateReboundPosition();
                    return; 
            }
        }

        private void CalculateReboundPosition()
        {
            ReboundMotion reboundMotion = _reboundMotionCalculator.CalculateNextReboundMotion(_lastReboundMotion);
            _lastReboundMotion = reboundMotion;

            Vector2D viewVectorXZ = _vectorHelper.ConvertDegreeToVector(reboundMotion.MainViewDegreeXZ + reboundMotion.RelativeViewDegreeXZ);
            SetMotionFields(reboundMotion.MainViewDegreeXZ + reboundMotion.RelativeViewDegreeXZ, reboundMotion.ViewDegreeY, viewVectorXZ);
        }

        private void CalculateDrivePosition()
        {
            if (_lastVehicleMotion == null)
            {
                _lastVehicleMotion = new VehicleMotion
                {
                    Position = _position,
                    Speed = 0.0,
                    SteeringWheelAngle = 0.0,
                    MainDegreeXZ = 0.0,
                    ViewDegreeY = 0,
                    RelativeDriveDegreeXZ = 0
                };
            }

            VehicleMotion vehicleMotion = _vehicleMotionCalculator.CalculateNextVehicleMotion(_lastVehicleMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(vehicleMotion.Position, _playerSideLength, _height))
            {
                _position = vehicleMotion.Position;
                _lastVehicleMotion = vehicleMotion;
            }
            else
            {
                _motionModus = MotionModus.Rebound;
                _lastReboundMotion = new ReboundMotion
                {
                    Speed = _lastVehicleMotion.Speed,
                    Position = _lastVehicleMotion.Position,
                    ViewDegreeY = _lastVehicleMotion.ViewDegreeY,
                    MainViewDegreeXZ = _lastVehicleMotion.MainDegreeXZ,
                    RelativeViewDegreeXZ = _lastVehicleMotion.RelativeDriveDegreeXZ,
                    MovementDegree = _lastVehicleMotion.MainDegreeXZ - 180
                };

                if (_lastReboundMotion.MovementDegree < 0)
                    _lastReboundMotion.MovementDegree += 360;
            }
            Vector2D viewVectorXZ = _vectorHelper.ConvertDegreeToVector(_lastVehicleMotion.MainDegreeXZ + _lastVehicleMotion.RelativeDriveDegreeXZ);
            SetMotionFields(_lastVehicleMotion.MainDegreeXZ + _lastVehicleMotion.RelativeDriveDegreeXZ, _lastVehicleMotion.ViewDegreeY, viewVectorXZ);
        }

        private void CalculateWalkPosition()
        {
            if (_lastWalkMotion == null)
            {
                _lastWalkMotion = new WalkMotion
                {
                    Position = _position
                };
            }

            WalkMotion walkMotion = _walkPositionCalculator.CalculateNextPosition(_lastWalkMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(walkMotion.Position, _playerSideLength, _height))
            {
                _position = walkMotion.Position;
                _lastWalkMotion = walkMotion;
            }

            SetMotionFields(walkMotion.DegreeXZ, walkMotion.DegreeY, walkMotion.VectorXZ);
        }

        private void SetMotionFields(double degreeXZ, double degreeY, Vector2D vectorXZ)
        {
            _direction = new ViewDirection { DegreeXZ = degreeXZ, DegreeY = degreeY };
            Vector2D vectorY = _vectorHelper.ConvertDegreeToVector(degreeY);
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
