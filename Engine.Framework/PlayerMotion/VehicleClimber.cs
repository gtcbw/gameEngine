using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class VehicleClimber : IVehicleClimber
    {
        private Position _positionPlayer;
        private Position _positionVehicle;
        private readonly IPercentProvider _percentProvider;

        public VehicleClimber(IPercentProvider percentProvider)
        {
            _percentProvider = percentProvider;
        }

        ClimbMotion IVehicleClimber.GetClimbUpPosition()
        {
            double percent = _percentProvider.GetPercent();

            Position interpolatedPosition = new Position
            {
                X = _positionPlayer.X * (1.0 - percent) + _positionVehicle.X * percent,
                Y = _positionPlayer.Y * (1.0 - percent) + _positionVehicle.Y * percent,
                Z = _positionPlayer.Z * (1.0 - percent) + _positionVehicle.Z * percent
            };

            return new ClimbMotion { Position = interpolatedPosition, Done = _percentProvider.IsOver() };
        }

        void IVehicleClimber.InitClimbUp(Position positionPlayer, Position positionVehicle)
        {
            _positionPlayer = positionPlayer;
            _positionVehicle = positionVehicle;
            _percentProvider.Start();
        }
    }
}
