using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleManager
    {
        IVehicle FindNearestVehicle(IReadOnlyPosition position);

        void EnterVehicle(IVehicle vehicle);

        void LeaveVehicle(IVehicle vehicle);
    }
}
