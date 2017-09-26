using Engine.Contracts.PlayerMotion;
using Engine.Contracts;
using World.Model;
using Math.Contracts;

namespace Engine.Framework
{
    public sealed class VehicleFinder : IVehicleFinder
    {
        private readonly IVehicleProvider _vehicleProvider;
        private readonly IPositionDistanceComparer _positionDistanceComparer;

        public VehicleFinder(IVehicleProvider vehicleProvider,
            IPositionDistanceComparer positionDistanceComparer)
        {
            _vehicleProvider = vehicleProvider;
            _positionDistanceComparer = positionDistanceComparer;
        }

        IVehicle IVehicleFinder.FindNearestVehicle(IReadOnlyPosition position)
        {
            foreach(IVehicle vehicle in _vehicleProvider.GetVehicles())
            {
                if (_positionDistanceComparer.PositionIsNearerThan(vehicle.Position, position, 2.5))
                    return vehicle;
            }

            return null;
        }
    }
}