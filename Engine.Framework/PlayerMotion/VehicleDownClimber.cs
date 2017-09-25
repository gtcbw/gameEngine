using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class VehicleDownClimber : IVehicleClimber
    {
        private IReadOnlyPosition _positionPlayer;
        private IReadOnlyPosition _positionVehicle;
        private double _degreeXZPlayer;
        private double _degreeYPlayer;
        private double _degreeXZVehicle;
        private double _degreeYVehicle;
        private readonly IPercentProvider _percentProvider;
        private readonly IPercentProvider _verticalSinus;

        public VehicleDownClimber(IPercentProvider percentProvider,
            IPercentProvider verticalSinus)
        {
            _percentProvider = percentProvider;
            _verticalSinus = verticalSinus;
        }

        ClimbMotion IVehicleClimber.GetClimbPosition()
        {
            double percent = _percentProvider.GetPercent();

            Position interpolatedPosition = new Position
            {
                X = _positionPlayer.X * percent + _positionVehicle.X * (1.0 - percent),
                Y = _positionPlayer.Y * percent + _positionVehicle.Y * (1.0 - percent),
                Z = _positionPlayer.Z * percent + _positionVehicle.Z * (1.0 - percent)
            };

            if (!_verticalSinus.IsOver())
            {
                double sinusPercent = _verticalSinus.GetPercent();
                double sinus = System.Math.Sin(sinusPercent * System.Math.PI);

                interpolatedPosition.Y += sinus * 0.8;
            }

            double interpolatedDegreeXZ = _degreeXZVehicle * (1.0 - percent) + _degreeXZPlayer * percent;
            if (interpolatedDegreeXZ > 359)
                interpolatedDegreeXZ -= 360;

            double interpolatedDegreeY;

            if (percent <= 0.5)
            {
                double percentY = 2 * percent;
                interpolatedDegreeY = _degreeYVehicle * (1.0 - percentY) + _degreeYPlayer * percentY;
            }
            else
                interpolatedDegreeY = _degreeYPlayer;

            return new ClimbMotion
            {
                Position = interpolatedPosition,
                DegreeXZ = interpolatedDegreeXZ,
                DegreeY = interpolatedDegreeY,
                Done = _percentProvider.IsOver()
            };
        }

        void IVehicleClimber.InitClimb(IReadOnlyPosition positionPlayer, double degreeXZPlayer, double degreeYPlayer, IReadOnlyPosition positionVehicle, double degreeXZVehicle, double degreeYVehicle)
        {
            _positionPlayer = positionPlayer;
            _positionVehicle = positionVehicle;
            _degreeXZPlayer = degreeXZPlayer;
            _degreeYPlayer = degreeYPlayer;
            _degreeXZVehicle = degreeXZVehicle;
            _degreeYVehicle = degreeYVehicle;
            _percentProvider.Start();
            _verticalSinus.Start();

            if (_degreeXZVehicle < _degreeXZPlayer)
            {
                double simpleDifference = _degreeXZPlayer - _degreeXZVehicle;
                double comparisonDifference = _degreeXZVehicle + 360 - _degreeXZPlayer;

                if (comparisonDifference < simpleDifference)
                    _degreeXZVehicle += 360;
            }
            else
            {
                double simpleDifference = _degreeXZVehicle - _degreeXZPlayer;
                double comparisonDifference = _degreeXZPlayer + 360 - _degreeXZVehicle;

                if (comparisonDifference < simpleDifference)
                    _degreeXZPlayer += 360;
            }
        }
    }
}
