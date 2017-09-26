using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleFinder
    {
        IVehicle FindNearestVehicle(IReadOnlyPosition position);
    }
}
