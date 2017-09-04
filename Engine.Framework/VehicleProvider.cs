using Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Framework
{
    public sealed class VehicleProvider : IVehicleProvider
    {
        IEnumerable<Vehicle> IVehicleProvider.GetVehicles()
        {
            return new List<Vehicle>();
        }
    }
}
