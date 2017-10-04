using Engine.Contracts;
using Engine.Contracts.Input;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class VehicleMotionCalculator : IVehicleMotionCalculator
    {
        private double _accelerationPerSecond = 15;
        private double _maxSpeed = 60;
        private double _maxSteeringWheelAngle = 25;
        private double _steeringAnglePerSecond = 15;
        private double _maxDegreeY = 70;
        private IVectorHelper _vectorHelper;
        private IMousePositionController _mousePositionController;
        private IKeyMapper _keyMapper;
        private IHeightCalculator _heightCalculator;
        private IFrameTimeProvider _frameTimeProvider;

        public VehicleMotionCalculator(IVectorHelper vectorHelper,
            IMousePositionController mousePositionController,
            IKeyMapper keyMapper,
            IHeightCalculator heightCalculator,
            IFrameTimeProvider frameTimeProvider)
        {
            _vectorHelper = vectorHelper;
            _mousePositionController = mousePositionController;
            _keyMapper = keyMapper;
            _heightCalculator = heightCalculator;
            _frameTimeProvider = frameTimeProvider;
        }

        VehicleMotion IVehicleMotionCalculator.CalculateNextVehicleMotion(VehicleMotion currentVehicleMotion)
        {
            VehicleMotion vehicleMotion = new VehicleMotion
            {
                SteeringWheelAngle = currentVehicleMotion.SteeringWheelAngle,
                Speed = currentVehicleMotion.Speed,
                MainDegreeXZ = currentVehicleMotion.MainDegreeXZ,
                ViewDegreeY = currentVehicleMotion.ViewDegreeY,
                RelativeDriveDegreeXZ = currentVehicleMotion.RelativeDriveDegreeXZ
            };

            var keys = _keyMapper.GetMappedKeys();

            if (keys.StrafeLeft)
            {
                vehicleMotion.SteeringWheelAngle -= _steeringAnglePerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (vehicleMotion.SteeringWheelAngle < -_maxSteeringWheelAngle)
                    vehicleMotion.SteeringWheelAngle = -_maxSteeringWheelAngle;
            }
            else if (keys.StrafeRight)
            {
                vehicleMotion.SteeringWheelAngle += _steeringAnglePerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (vehicleMotion.SteeringWheelAngle > _maxSteeringWheelAngle)
                    vehicleMotion.SteeringWheelAngle = _maxSteeringWheelAngle;
            }
            else
            {
                if (vehicleMotion.SteeringWheelAngle > 0)
                {
                    vehicleMotion.SteeringWheelAngle -= _steeringAnglePerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                    if (vehicleMotion.SteeringWheelAngle < 0)
                        vehicleMotion.SteeringWheelAngle = 0;
                }
                else if (vehicleMotion.SteeringWheelAngle < 0)
                {
                    vehicleMotion.SteeringWheelAngle += _steeringAnglePerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                    if (vehicleMotion.SteeringWheelAngle > 0)
                        vehicleMotion.SteeringWheelAngle = 0;
                }
            }

            if (keys.WalkForward)
            {
                vehicleMotion.Speed += _accelerationPerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (vehicleMotion.Speed > _maxSpeed)
                    vehicleMotion.Speed = _maxSpeed;
            }
            else if (keys.WalkBackward)
            {
                vehicleMotion.Speed -= _accelerationPerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();
                if (vehicleMotion.Speed < -_maxSpeed / 2.0)
                    vehicleMotion.Speed = -_maxSpeed / 2.0;
            }
            else
            {
                if (vehicleMotion.Speed > 0)
                {
                    vehicleMotion.Speed -= _accelerationPerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() / 4.0;
                    if (vehicleMotion.Speed < 0)
                        vehicleMotion.Speed = 0;
                }
                else if (vehicleMotion.Speed < 0)
                {
                    vehicleMotion.Speed += _accelerationPerSecond * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() / 4.0;
                    if (vehicleMotion.Speed > 0)
                        vehicleMotion.Speed = 0;
                }
            }

            vehicleMotion.MainDegreeXZ += vehicleMotion.SteeringWheelAngle * vehicleMotion.Speed / 10.0 * _frameTimeProvider.GetTimeInSecondsSinceLastFrame();

            if (vehicleMotion.MainDegreeXZ > 360.0)
                vehicleMotion.MainDegreeXZ -= 360.0;
            else if (vehicleMotion.MainDegreeXZ < 0.0)
                vehicleMotion.MainDegreeXZ += 360.0;

            MousePositionDelta mousePositionDelta = _mousePositionController.GetMousePositionDelta();

            vehicleMotion.RelativeDriveDegreeXZ += mousePositionDelta.PositionDeltaX;

            vehicleMotion.ViewDegreeY += mousePositionDelta.PositionDeltaY;

            if (vehicleMotion.RelativeDriveDegreeXZ > 90.0)
                vehicleMotion.RelativeDriveDegreeXZ = 90.0;
            else if (vehicleMotion.RelativeDriveDegreeXZ < -90.0)
                vehicleMotion.RelativeDriveDegreeXZ = -90.0;

            if (vehicleMotion.ViewDegreeY > _maxDegreeY)
                vehicleMotion.ViewDegreeY = _maxDegreeY;
            else if (vehicleMotion.ViewDegreeY < 0)
                vehicleMotion.ViewDegreeY = 0;

            Vector2D movementVector = _vectorHelper.ConvertDegreeToVector(vehicleMotion.MainDegreeXZ);
            
            vehicleMotion.Position = new Position { X = currentVehicleMotion.Position.X, Z = currentVehicleMotion.Position.Z };
            vehicleMotion.Position.X += movementVector.X * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * vehicleMotion.Speed;
            vehicleMotion.Position.Z += movementVector.Z * _frameTimeProvider.GetTimeInSecondsSinceLastFrame() * vehicleMotion.Speed;
            vehicleMotion.Position.Y = _heightCalculator.CalculateHeight(vehicleMotion.Position.X, vehicleMotion.Position.Z);

            return vehicleMotion;
        }
    }
}
