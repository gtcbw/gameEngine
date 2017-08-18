using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class RayWithMapTester
    {
        private IHeightCalculator _heightCalculator;
        private double _maxTestDistance;

        public RayWithMapTester(IHeightCalculator heightCalculator, double maxTestDistance)
        {
            _heightCalculator = heightCalculator;
            _maxTestDistance = maxTestDistance;
        }

        public Position FindCollisionWithMap(Ray ray)
        {
            double testedDistance = 0;

            double[] x_y_z = new double[] { ray.StartPosition.X, ray.StartPosition.Y, ray.StartPosition.Z };

            double height = _heightCalculator.CalculateHeight(x_y_z[0], x_y_z[2]);

            if (height < x_y_z[1])
                return new Position { X = x_y_z[0], Y = height, Z = x_y_z[2] };

            double vectorChangeLength;

            while(testedDistance < _maxTestDistance)
            {
                if (ray.Direction.Y > 0)
                    vectorChangeLength = 2 * height;
                else
                    vectorChangeLength = 0.5 * height;

                if (vectorChangeLength > 10)
                    vectorChangeLength = 10;
                else if (vectorChangeLength < 0.1)
                    vectorChangeLength = 0.1;

                x_y_z[0] += ray.Direction.X * vectorChangeLength;
                x_y_z[1] += ray.Direction.X * vectorChangeLength;
                x_y_z[2] += ray.Direction.Z * vectorChangeLength;
                testedDistance += vectorChangeLength;

                height = _heightCalculator.CalculateHeight(x_y_z[0], x_y_z[2]);

                if (height < x_y_z[1])
                    return new Position { X = x_y_z[0], Y = height, Z = x_y_z[2] };
            }

            return null;
        }
    }
}
