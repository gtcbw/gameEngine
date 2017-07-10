using Math.Contracts;
using World.Model;

namespace Math
{
    public sealed class VectorHelper : IVectorHelper
    {
        Vector IVectorHelper.CalculateUnitLengthVector(Vector vector)
        {
            double squareRoot = System.Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y) + (vector.Z * vector.Z));

            return new Vector
            {
                X = vector.X / squareRoot,
                Y = vector.Y / squareRoot,
                Z = vector.Z / squareRoot
            };
        }

        Vector IVectorHelper.CreateFromDegrees(double degreeXZ, double degreeY)
        {
            Vector mainVector = ConvertDegreeToFlatVector(degreeXZ);
            Vector yVector = ConvertDegreeToFlatVector(degreeY);

            Vector vector = new Vector();
            vector.X = mainVector.X * yVector.X;
            vector.Z = mainVector.Z * yVector.X;
            vector.Y = yVector.Z;

            return vector;
        }

        private Vector ConvertDegreeToFlatVector(double degree)
        {
            Vector vector = new Vector();

            if (degree > 360)
            {
                degree -= 360;

                if (degree > 720)
                {
                    degree = ((int)degree) % 360;
                }
            }
            else if (degree < 0)
            {
                degree += 360;

                if (degree < -360)
                {
                    degree = -(((int)-degree) % 360);

                    degree += 360;
                }
            }

            double rad = degree / 180 * System.Math.PI;

            vector.X = System.Math.Cos(rad);
            vector.Z = System.Math.Sin(rad);

            return vector;
        }
    }
}
