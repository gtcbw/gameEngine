using System.Collections.Generic;

namespace Engine.Contracts
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAllVehicles();
    }
}
