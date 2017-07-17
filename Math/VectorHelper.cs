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
            Vector2D mainVector = ConvertDegreeToFlatVector(degreeXZ);
            Vector2D yVector = ConvertDegreeToFlatVector(degreeY);

            Vector vector = new Vector
            {
                X = mainVector.X * yVector.X,
                Z = mainVector.Z * yVector.X,
                Y = yVector.Z
            };
            return vector;
        }

        private Vector2D ConvertDegreeToFlatVector(double degree)
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

        Vector2D IVectorHelper.Rotate90Degree(Vector2D vector)
        {
            return new Vector2D { X = -vector.Z, Z = vector.X };
        }

        Vector2D IVectorHelper.Rotate180Degree(Vector2D vector)
        {
            return new Vector2D { X = -vector.X, Z = -vector.Z };
        }

        Vector2D IVectorHelper.Rotate270Degree(Vector2D vector)
        {
            return new Vector2D { X = vector.Z, Z = -vector.X };
        }

        Vector2D IVectorHelper.MixTwoRectangularVectors(Vector2D vectorOne, Vector2D vectorTwo)
        {
            return new Vector2D
            {
                X = (vectorOne.X * 0.7070106) + (vectorTwo.X * 0.7070106),
                Z = (vectorOne.Z * 0.7070106) + (vectorTwo.Z * 0.7070106)
            };
        }
    }
}
