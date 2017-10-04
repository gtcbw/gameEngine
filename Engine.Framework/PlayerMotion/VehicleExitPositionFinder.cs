using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Framework.PlayerMotion
{
    public sealed class VehicleExitPositionFinder : IVehicleExitPositionFinder
    {
        private readonly double _distanceToVehicleCenter;
        private readonly IVectorHelper _vectorHelper;
        private readonly ICuboidWithWorldTester _cuboidWithWorldTester;
        private readonly IHeightCalculator _heightCalculator;

        public VehicleExitPositionFinder(double distanceToVehicleCenter, 
            IVectorHelper vectorHelper,
            ICuboidWithWorldTester cuboidWithWorldTester, 
            IHeightCalculator heightCalculator)
        {
            _distanceToVehicleCenter = distanceToVehicleCenter;
            _vectorHelper = vectorHelper;
           _cuboidWithWorldTester = cuboidWithWorldTester;
            _heightCalculator = heightCalculator;
        }

        Position IVehicleExitPositionFinder.FindPosition(IReadOnlyPosition vehiclePosition, double vehicleRotationXZ)
        {
            for (int i = 0; i < 6; i++)
            {
                double exitDegree = vehicleRotationXZ;

                switch(i)
                {
                    case 0:
                        exitDegree -= 30;
                        break;
                    case 1:
                        exitDegree += 30;
                        break;
                    case 2:
                        exitDegree -= 90;
                        break;
                    case 3:
                        exitDegree += 90;
                        break;
                    case 4:
                        exitDegree -= 150;
                        break;
                    case 5:
                        exitDegree += 150;
                        break;
                }
                Position position = CalculateExitPosition(vehiclePosition, exitDegree);

                if (!_cuboidWithWorldTester.ElementCollidesWithWorld(position, 0.7, 1.9))
                    return position;
            }
            return null;
        }

        private Position CalculateExitPosition(IReadOnlyPosition vehiclePosition, double exitDegree)
        {
            var vector = _vectorHelper.ConvertDegreeToVector(exitDegree);

            return new Position
            {
                X = vehiclePosition.X + vector.X * _distanceToVehicleCenter,
                Y = _heightCalculator.CalculateHeight(vehiclePosition.X + vector.X * _distanceToVehicleCenter, vehiclePosition.Z + vector.Z * _distanceToVehicleCenter),
                Z = vehiclePosition.Z + vector.Z * _distanceToVehicleCenter
            };
        }
    }
}
