using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class VehicleUpClimber : IVehicleClimber
    {
        private IReadOnlyPosition _positionPlayer;
        private IReadOnlyPosition _positionVehicle;
        private double _degreeXZPlayer;
        private double _degreeYPlayer;
        private double _degreeXZVehicle;
        private double _degreeYVehicle;
        private readonly IPercentProvider _percentProvider;
        private readonly IPercentProvider _verticalDelay;
        private readonly IPercentProvider _verticalSinus;
        private bool _sinusStarted;

        public VehicleUpClimber(IPercentProvider percentProvider,
            IPercentProvider verticalDelay,
            IPercentProvider verticalSinus)
        {
            _percentProvider = percentProvider;
            _verticalDelay = verticalDelay;
            _verticalSinus = verticalSinus;
        }

        ClimbMotion IVehicleClimber.GetClimbPosition()
        {
            double percent = _percentProvider.GetPercent();
            _verticalDelay.GetPercent();

            if (!_sinusStarted && _verticalDelay.IsOver())
            {
                _verticalSinus.Start();
                _sinusStarted = true;
            }

            Position interpolatedPosition = new Position
            {
                X = _positionPlayer.X * (1.0 - percent) + _positionVehicle.X * percent,
                Y = _positionPlayer.Y * (1.0 - percent) + _positionVehicle.Y * percent,
                Z = _positionPlayer.Z * (1.0 - percent) + _positionVehicle.Z * percent
            };

            if (_sinusStarted)
            {
                double sinusPercent = _verticalSinus.GetPercent();
                double sinus = System.Math.Sin(sinusPercent * System.Math.PI);

                interpolatedPosition.Y += sinus * 0.8;
            }

            double interpolatedDegreeXZ = _degreeXZPlayer * (1.0 - percent) + _degreeXZVehicle * percent;
            if (interpolatedDegreeXZ > 359)
                interpolatedDegreeXZ -= 360;

            double interpolatedDegreeY;

            if (percent <= 0.5)
            {
                double percentY = 2 * percent;
                interpolatedDegreeY = _degreeYPlayer * (1.0 - percentY) + _degreeYVehicle * percentY;
            }
            else
                interpolatedDegreeY = _degreeYVehicle;

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
            _verticalDelay.Start();
            _sinusStarted = false;

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
