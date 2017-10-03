namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleUsageObserver
    {
        void ClimbingDownVehicle();

        void ClimbingUpVehicle();

        void SetDriveParameters(double speed, double deltaDegreeXZ, double deltaDegreeY);
    }
}
