using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System.Collections.Generic;

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
                        Position = new World.Model.Position { X = 80, Y = _heightCalculator.CalculateHeight(80, 80), Z = 80 },
                        ComplexShape = new World.Model.ComplexShape
                        {
                            Faces = new World.Model.Face[0],
                            RadiusXZ = 1.5,
                            MainCuboid = new World.Model.Cuboid
                            {
                                Center = new World.Model.Position(),
                                SideLengthX = 1.5,
                                SideLengthY = 1,
                                SideLengthZ = 0.5
                            }
                        }
                    }
                }
            };
        }
    }
}
