using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleExitPositionFinder
    {
        Position FindPosition(IReadOnlyPosition vehiclePosition, double vehicleRotationXZ);
    }
}
