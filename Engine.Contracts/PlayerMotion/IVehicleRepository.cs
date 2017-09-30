using System.Collections.Generic;

namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAllVehicles();
    }
}
