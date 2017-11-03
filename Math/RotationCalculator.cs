using Math.Contracts;
using System;

using World.Model;

namespace Math
{
    public sealed class RotationCalculator : IRotationCalculator
    {
        TwoComponentRotation IRotationCalculator.CalculateRotation(Vector vector)
        {
            double degreeXZ = CalculateXZ(vector);
            double degreeY = CalculateY(vector);

            return new TwoComponentRotation
            {
                DegreeXZ = degreeXZ
            };
        }

        private double CalculateY(Vector vector)
        {
            double lengthXZSquare = vector.X * vector.X + vector.Z * vector.Z;

            if (lengthXZSquare < 0.001 && lengthXZSquare > -0.001)
            {
                if (vector.Y > 0)
                    return 90;
                return 270;
            }

            double lengthXZ = System.Math.Sqrt(lengthXZSquare);
            var rad = System.Math.Atan(lengthXZ / vector.Y);

            double degreeY = rad * 180 / System.Math.PI;
            if (degreeY < 0)
                degreeY += 360;

            return degreeY;
        }

        private static double CalculateXZ(Vector vector)
        {
            double tan = vector.X != 0.0 ? vector.Z / vector.X : 20001;

            double degreeXZ;

            if (tan > 20000 || tan < -20000)
            {
                if (vector.Z < 0.0)
                    return 270;
                return 90;
            }

            var rad = System.Math.Atan(tan);

            degreeXZ = rad * 180 / System.Math.PI;

            if (vector.X > 0)
            {
                if (degreeXZ < 0)
                    degreeXZ += 360;
            }
            else
                degreeXZ += 180;

            return 360 - degreeXZ;
        }
    }
}
