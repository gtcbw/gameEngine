using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Framework
{
    public sealed class VehicleUsageRenderer : IVehicleUsageObserver, IRenderingElement
    {
        private readonly IRenderingElement _vehicleOnScreen;
        private readonly IWorldTranslator _worldTranslator;
        private double _speed;
        private double _deltaDegreeXZ;
        private double _deltaDegreeY;

        public VehicleUsageRenderer(IRenderingElement vehicleOnScreen,
            IWorldTranslator worldTranslator)
        {
            _vehicleOnScreen = vehicleOnScreen;
            _worldTranslator = worldTranslator;
        }

        void IVehicleUsageObserver.ClimbingDownVehicle()
        {
        }

        void IVehicleUsageObserver.ClimbingUpVehicle()
        {
        }

        void IVehicleUsageObserver.SetDriveParameters(double speed, double deltaDegreeXZ, double deltaDegreeY)
        {
            _speed = speed;
            _deltaDegreeXZ = deltaDegreeXZ;
            _deltaDegreeY = deltaDegreeY;
        }

        void IRenderingElement.Render()
        {
            _worldTranslator.Store();
            _worldTranslator.TranslateWorld(-_deltaDegreeXZ / 90.0, -_deltaDegreeY / 90.0, 0);
            _vehicleOnScreen.Render();
            _worldTranslator.Reset();
        }
    }
}
