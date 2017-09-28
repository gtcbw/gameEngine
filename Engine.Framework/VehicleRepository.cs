using Engine.Contracts;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Framework
{
    public sealed class VehicleRepository : IVehicleRepository
    {
        private readonly IHeightCalculator _heightCalculator;

        public VehicleRepository(IHeightCalculator heightCalculator)
        {
            _heightCalculator = heightCalculator;
        }

        IEnumerable<Vehicle> IVehicleRepository.GetAllVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle
                {
                    CollisionModel = new World.Model.ComplexShapeInstance
                    {
                        Position = new World.Model.Position { X = 100, Y = _heightCalculator.CalculateHeight(100, 100), Z = 100 }
                    }
                }
            };
        }
    }
}
