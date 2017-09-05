using Engine.Contracts.PlayerMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Framework.PlayerMotion
{
    public sealed class VehicleMotionCalculator : IVehicleMotionCalculator
    {
        VehicleMotion IVehicleMotionCalculator.CalculateNextVehicleMotion(VehicleMotion currentVehicleMotion)
        {
            return new VehicleMotion
            {
                Position = new World.Model.Position
                {
                    X = currentVehicleMotion.Position.X,
                    Y = currentVehicleMotion.Position.Y,
                    Z = currentVehicleMotion.Position.Z
                },
                Speed = 0,
                MainDegreeXZ = currentVehicleMotion.MainDegreeXZ,
                SteeringWheelAngle = 0
            };
        }
    }
}
