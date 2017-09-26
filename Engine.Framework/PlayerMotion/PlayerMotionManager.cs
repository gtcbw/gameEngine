using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class PlayerMotionManager
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

        private readonly IPlayerMotionEncapsulator _playerMotionEncapsulator;
        private readonly IWalkPositionCalculator _walkPositionCalculator;
        private readonly ICuboidWithWorldTester _cuboidWithWorldTester;
        private readonly IPressedKeyEncapsulator _enteredVehicleKey;
        private readonly IVehicleMotionCalculator _vehicleMotionCalculator;
        private readonly IReboundMotionCalculator _reboundMotionCalculator;
        private readonly IFrameTimeProvider _frameTimeProvider;
        private readonly IVehicleFinder _vehicleFinder;
        private readonly IVehicleClimber _vehicleUpClimber;
        private readonly IVehicleClimber _vehicleDownClimber;
        private double _height = 1.8;
        private double _playerSideLength = 0.6;
        private MotionModus _motionModus;
        private VehicleMotion _lastVehicleMotion;
        private WalkMotion _lastWalkMotion;
        private ReboundMotion _lastReboundMotion;

        private IVehicle _vehicle;

        public PlayerMotionManager(IPlayerMotionEncapsulator playerMotionEncapsulator,
            IWalkPositionCalculator walkPositionCalculator,
            ICuboidWithWorldTester cuboidWithWorldTester,
            IPressedKeyEncapsulator enteredVehicleKey,
            IVehicleMotionCalculator vehicleMotionCalculator,
            IReboundMotionCalculator reboundMotionCalculator,
            IFrameTimeProvider frameTimeProvider,
            IVehicleFinder vehicleFinder,
            IVehicleClimber vehicleUpClimber,
            IVehicleClimber vehicleDownClimber)
        {
            _playerMotionEncapsulator = playerMotionEncapsulator;
            _walkPositionCalculator = walkPositionCalculator;
            _cuboidWithWorldTester = cuboidWithWorldTester;
            _enteredVehicleKey = enteredVehicleKey;
            _vehicleMotionCalculator = vehicleMotionCalculator;
            _reboundMotionCalculator = reboundMotionCalculator;
            _frameTimeProvider = frameTimeProvider;
            _vehicleFinder = vehicleFinder;
            _vehicleUpClimber = vehicleUpClimber;
            _vehicleDownClimber = vehicleDownClimber;
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
            _vehicle = _vehicleFinder.FindNearestVehicle(_playerMotionEncapsulator.GetPlayerPosition());
            if (_vehicle == null)
                return;
            _motionModus = MotionModus.ClimbUp;
            _vehicleUpClimber.InitClimb(_playerMotionEncapsulator.GetPlayerPosition(),
                _playerMotionEncapsulator.GetViewDirection().DegreeXZ,
                _playerMotionEncapsulator.GetViewDirection().DegreeY,
                _vehicle.Position, 
                _vehicle.DegreeXZ, 
                0.0);
            _lastWalkMotion = null;
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
            Position playerPosition = _playerMotionEncapsulator.GetPlayerPosition().Clone();
            playerPosition.X += 1.5;
            //TODO: calculate hieght of new position
            _vehicleDownClimber.InitClimb(playerPosition,
                _playerMotionEncapsulator.GetViewDirection().DegreeXZ,
                0, 
                _playerMotionEncapsulator.GetPlayerPosition(),
                _playerMotionEncapsulator.GetViewDirection().DegreeXZ, 
                _playerMotionEncapsulator.GetViewDirection().DegreeY);
            _lastVehicleMotion = null;
        }

        private void ClimbDown()
        {
            var motion = _vehicleDownClimber.GetClimbPosition();

            _playerMotionEncapsulator.SetMotion(motion.Position, _height, motion.DegreeXZ, motion.DegreeY);

            if (motion.Done)
                _motionModus = MotionModus.Walk;
        }

        private void ClimbUp()
        {
            var motion = _vehicleUpClimber.GetClimbPosition();

            _playerMotionEncapsulator.SetMotion(motion.Position, _height ,motion.DegreeXZ, motion.DegreeY);

            if (motion.Done) 
                _motionModus = MotionModus.Drive;
        }

        private void CalculateReboundPosition()
        {
            ReboundMotion reboundMotion = _reboundMotionCalculator.CalculateNextReboundMotion(_lastReboundMotion);

            if (_cuboidWithWorldTester.ElementCollidesWithWorld(reboundMotion.Position, _playerSideLength, _height))
            {
                reboundMotion.Position = _playerMotionEncapsulator.GetPlayerPosition().Clone();
                reboundMotion.MovementDegree += 90.0;
                if (reboundMotion.MovementDegree > 359)
                    reboundMotion.MovementDegree -= 360;
            }
            _lastReboundMotion = reboundMotion;

            _playerMotionEncapsulator.SetMotion(reboundMotion.Position, _height, reboundMotion.MainViewDegreeXZ + reboundMotion.RelativeViewDegreeXZ, reboundMotion.ViewDegreeY);
            _vehicle.UpdatePosition(reboundMotion.Position, reboundMotion.MainViewDegreeXZ);
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

        private void CalculateDrivePosition()
        {
            if (_lastVehicleMotion == null)
                _lastVehicleMotion = new VehicleMotion
                {
                    Position = _playerMotionEncapsulator.GetPlayerPosition().Clone(),
                    MainDegreeXZ = _playerMotionEncapsulator.GetViewDirection().DegreeXZ
                };

            VehicleMotion vehicleMotion = _vehicleMotionCalculator.CalculateNextVehicleMotion(_lastVehicleMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(vehicleMotion.Position, _playerSideLength, _height))
            {
                _lastVehicleMotion = vehicleMotion;
                _playerMotionEncapsulator.SetMotion(_lastVehicleMotion.Position, _height, _lastVehicleMotion.MainDegreeXZ + _lastVehicleMotion.RelativeDriveDegreeXZ, _lastVehicleMotion.ViewDegreeY);
                _vehicle.UpdatePosition(_lastVehicleMotion.Position, _lastVehicleMotion.MainDegreeXZ);
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
                    Position = _playerMotionEncapsulator.GetPlayerPosition().Clone(),
                    DegreeXZ = _playerMotionEncapsulator.GetViewDirection().DegreeXZ,
                    DegreeY = _playerMotionEncapsulator.GetViewDirection().DegreeY
                };

            WalkMotion walkMotion = _walkPositionCalculator.CalculateNextPosition(_lastWalkMotion);

            if (!_cuboidWithWorldTester.ElementCollidesWithWorld(walkMotion.Position, _playerSideLength, _height))
            {
                _lastWalkMotion = walkMotion;
            }

            _playerMotionEncapsulator.SetMotion(_lastWalkMotion.Position, _height, walkMotion.DegreeXZ, walkMotion.DegreeY, walkMotion.VectorXZ);
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
