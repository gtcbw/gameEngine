using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;

namespace Engine.Framework
{
    public sealed class VehicleUsageRenderer : IVehicleUsageObserver, IRenderingElement
    {
        private readonly IRenderingElement _vehicleOnScreen;
        private readonly ITranslator _worldTranslator;
        private readonly IMatrixManager _matrixManager;
        private readonly IPercentProvider _climbUpDelay;
        private readonly IPercentProvider _climbTimer;
        private double _speed;
        private double _deltaDegreeXZ;
        private double _deltaDegreeY;
        private bool _entered;
        private bool _climbingDown;
        private bool _parametersAreUpdated;

        public VehicleUsageRenderer(IRenderingElement vehicleOnScreen,
            ITranslator worldTranslator,
            IMatrixManager matrixManager,
            IPercentProvider climbUpDelay,
            IPercentProvider climbTimer)
        {
            _vehicleOnScreen = vehicleOnScreen;
            _worldTranslator = worldTranslator;
            _matrixManager = matrixManager;
            _climbUpDelay = climbUpDelay;
            _climbTimer = climbTimer;
        }

        void IVehicleUsageObserver.ClimbingDownVehicle()
        {
            _climbTimer.Start();
            _climbingDown = true;
        }

        void IVehicleUsageObserver.ClimbingUpVehicle()
        {
            _entered = true;
            _parametersAreUpdated = false;
            _climbUpDelay.Start();
        }

        void IVehicleUsageObserver.SetDriveParameters(double speed, double deltaDegreeXZ, double deltaDegreeY)
        {
            _speed = speed;
            _deltaDegreeXZ = deltaDegreeXZ;
            _deltaDegreeY = deltaDegreeY;
            _parametersAreUpdated = true;
        }

        void IRenderingElement.Render()
        {
            if (!_entered)
                return;

            if (!_climbUpDelay.IsOver())
            {
                _climbUpDelay.GetPercent();
                if (_climbUpDelay.IsOver())
                    _climbTimer.Start();

                return;
            }

            double percent = _climbTimer.GetPercent();

            if (_climbingDown)
            {
                percent = 1 - percent;

                if (_climbTimer.IsOver())
                {
                    _climbingDown = false;
                    _entered = false;
                    return;
                }
            }

            _matrixManager.Store();

            double translateX = 0;
            double translateY = 0;

            if (_parametersAreUpdated)
            {
                translateX = -_deltaDegreeXZ / 90.0;
                translateY = -_deltaDegreeY / 90.0;
            }

            if (!_climbTimer.IsOver())
                _worldTranslator.Translate(_climbingDown ? translateX : 0, -0.5 + percent / 2.0 + (_climbingDown ? translateY : 0), 0); 
            else
                _worldTranslator.Translate(translateX, translateY, 0);

            _vehicleOnScreen.Render();
            _matrixManager.Reset();
        }
    }
}
