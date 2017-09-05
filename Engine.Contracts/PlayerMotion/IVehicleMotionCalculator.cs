namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleMotionCalculator
    {
        VehicleMotion CalculateNextVehicleMotion(VehicleMotion currentVehicleMotion);
    }
}
