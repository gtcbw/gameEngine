using Engine.Contracts.PlayerMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Contracts;
using World.Model;

namespace Engine.Framework
{
    public sealed class VehicleFinder : IVehicleFinder
    {
        private readonly IVehicleProvider _vehicleProvider;

        public VehicleFinder(IVehicleProvider vehicleProvider)
        {
            _vehicleProvider = vehicleProvider;
        }

        Vehicle IVehicleFinder.FindNearestVehicle(IReadOnlyPosition position)
        {
            return new Vehicle
            {
                Position = new Position
                {
                    X = position.X + 1.0,
                    Y = position.Y,
                    Z = position.Z + 1.0
                }
            };
        }
    }
}
