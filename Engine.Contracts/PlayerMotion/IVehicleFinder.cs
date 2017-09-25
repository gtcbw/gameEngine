using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleFinder
    {
        Vehicle FindNearestVehicle(IReadOnlyPosition position);
    }
}
