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
            Rebound = 2,
            ClimbUp = 3,
            ClimbDown = 4,
            FullBrake = 5
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
        private readonly IVehicleFinder _vehicleFinder;
        private readonly IVehicleClimber _vehicleUpClimber;
        private readonly IVehicleClimber _vehicleDownClimber;
        private Position _position;
        private Ray _ray = new Ray();
        ViewDirection _direction;
        private double _height = 1.8;
        private double _playerSideLength = 0.6;
        private MotionModus _motionModus;
        private VehicleMotion _lastVehicleMotion;
        private WalkMotion _lastWalkMotion;
        private ReboundMotion _lastReboundMotion;

        private Vehicle _vehicle;

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
            IVehicleFinder vehicleFinder,
            IVehicleClimber vehicleUpClimber,
            IVehicleClimber vehicleDownClimber,
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
            _vehicleFinder = vehicleFinder;
            _vehicleUpClimber = vehicleUpClimber;
            _vehicleDownClimber = vehicleDownClimber;
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
                        EnterVehicle();
                    return;
                case MotionModus.ClimbUp:
                    ClimbUp();
                    return;
                case MotionModus.ClimbDown:
                    ClimbDown();
                    return;
                case MotionModus.FullBrake:
                    Brake();
                    return;
                case MotionModus.Drive:
                    CalculateDrivePosition();
                    if (_enteredVehicleKey.KeyWasPressedOnce())
                        ExitVehicle();
                    return;
                case MotionModus.Rebound:
                    CalculateReboundPosition();
                    if (_lastReboundMotion.Speed < 2)
                    {
                        _motionModus = MotionModus.Drive;
                        _lastVehicleMotion = Convert(_lastReboundMotion);
                        _lastReboundMotion = null;
                    }
                    return; 
            }
        }



        private void EnterVehicle()
        {
            _vehicle = _vehicleFinder.FindNearestVehicle(_position);
            if (_vehicle == null)
                return;
            _motionModus = MotionModus.ClimbUp;
            _vehicleUpClimber.InitClimb(_position.Clone(), _direction.DegreeXZ, _direction.DegreeY, _vehicle.Position.Clone(), _vehicle.DegreeXZ, 0.0);
            _lastWalkMotion = null;
        }

        private void Brake()
        {
            _lastVehicleMotion.Speed -= _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * 30;

            if (_lastVehicleMotion.Speed < 0)
                _lastVehicleMotion.Speed = 0;

            CalculateDrivePosition();

            if (_lastVehicleMotion.Speed < 0.5)
                InitClimbDown();
        }

        private void ExitVehicle()
        {
            if (_lastVehicleMotion.Speed > 0.5)
            {
                _motionModus = MotionModus.FullBrake;
                return;
            }
            InitClimbDown();
        }

        private void InitClimbDown()
        {
            _motionModus = MotionModus.ClimbDown;
            Position playerPosition = _position.Clone();
            playerPosition.X += 1.5;
            _vehicleDownClimber.InitClimb(_position.Clone(), _direction.DegreeXZ, _direction.DegreeY, playerPosition, _direction.DegreeXZ, 0.0);
            _lastVehicleMotion = null;
        }

        private void ClimbDown()
        {
            var motion = _vehicleDownClimber.GetClimbPosition();
            _position = motion.Position;

            SetMotionFields(motion.DegreeXZ, motion.DegreeY);

            if (motion.Done)
                _motionModus = MotionModus.Walk;
        }

        private void ClimbUp()
        {
            var motion = _vehicleUpClimber.GetClimbPosition();
            _position = motion.Position;

            SetMotionFields(motion.DegreeXZ, motion.DegreeY);

            if (motion.Done) 
                _motionModus = MotionModus.Drive;
        }

        private void CalculateReboundPosition()
        {
            ReboundMotion reboundMotion = _reboundMotionCalculator.CalculateNextReboundMotion(_lastReboundMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(reboundMotion.Position, _playerSideLength, _height))
            {
                _position = reboundMotion.Position;
            }
            else
            {
                reboundMotion.Position = _position;
                reboundMotion.MovementDegree += 90.0;
                if (reboundMotion.MovementDegree > 359)
                    reboundMotion.MovementDegree -= 360;
            }
            _lastReboundMotion = reboundMotion;

            SetMotionFields(reboundMotion.MainViewDegreeXZ + reboundMotion.RelativeViewDegreeXZ, reboundMotion.ViewDegreeY);
        }

        private void CalculateDrivePosition()
        {
            if (_lastVehicleMotion == null)
                _lastVehicleMotion = new VehicleMotion{ Position = _position };

            VehicleMotion vehicleMotion = _vehicleMotionCalculator.CalculateNextVehicleMotion(_lastVehicleMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(vehicleMotion.Position, _playerSideLength, _height))
            {
                _position = vehicleMotion.Position;
                _lastVehicleMotion = vehicleMotion;
                SetMotionFields(_lastVehicleMotion.MainDegreeXZ + _lastVehicleMotion.RelativeDriveDegreeXZ, _lastVehicleMotion.ViewDegreeY);
            }
            else
            {
                _motionModus = MotionModus.Rebound;
                _lastReboundMotion = Convert(_lastVehicleMotion);
                _lastVehicleMotion = null;
            }
        }

        private void CalculateWalkPosition()
        {
            if (_lastWalkMotion == null)
                _lastWalkMotion = new WalkMotion
                {
                    Position = _position,
                    DegreeXZ = _direction?.DegreeXZ ?? 0,
                    DegreeY = _direction?.DegreeY ?? 0
                };

            WalkMotion walkMotion = _walkPositionCalculator.CalculateNextPosition(_lastWalkMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(walkMotion.Position, _playerSideLength, _height))
            {
                _position = walkMotion.Position;
                _lastWalkMotion = walkMotion;
            }

            SetMotionFields(walkMotion.DegreeXZ, walkMotion.DegreeY, walkMotion.VectorXZ);
        }

        private void SetMotionFields(double degreeXZ, double degreeY, Vector2D vectorXZ = null)
        {
            if (vectorXZ == null)
                vectorXZ = _vectorHelper.ConvertDegreeToVector(degreeXZ);

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

        private VehicleMotion Convert(ReboundMotion reboundMotion)
        {
            return new VehicleMotion
            {
                Speed = reboundMotion.Speed,
                Position = reboundMotion.Position,
                ViewDegreeY = reboundMotion.ViewDegreeY,
                MainDegreeXZ = reboundMotion.MainViewDegreeXZ,
                RelativeDriveDegreeXZ = reboundMotion.RelativeViewDegreeXZ,
                SteeringWheelAngle = 0
            };
        }

        private ReboundMotion Convert(VehicleMotion vehicleMotion)
        {
            var reboundMotion = new ReboundMotion
            {
                Speed = vehicleMotion.Speed,
                Position = vehicleMotion.Position,
                ViewDegreeY = vehicleMotion.ViewDegreeY,
                MainViewDegreeXZ = vehicleMotion.MainDegreeXZ,
                RelativeViewDegreeXZ = vehicleMotion.RelativeDriveDegreeXZ,
                MovementDegree = vehicleMotion.MainDegreeXZ - 180
            };

            if (reboundMotion.MovementDegree < 0)
                reboundMotion.MovementDegree += 360;

            return reboundMotion;
        }
    }
}
