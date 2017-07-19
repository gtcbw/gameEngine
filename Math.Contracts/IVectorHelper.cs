using World.Model;

namespace Math.Contracts
{
    public interface IVectorHelper
    {
        Vector CalculateUnitLengthVector(Vector vector);
        Vector CreateFromDegrees(double degreeXZ, double degreeY);
        Vector2D ConvertDegreeToVector(double degree);
        Vector2D Rotate90Degree(Vector2D vector);
        Vector2D Rotate180Degree(Vector2D vector);
        Vector2D Rotate270Degree(Vector2D vector);
        Vector2D MixTwoRectangularVectors(Vector2D vectorOne, Vector2D vectorTwo);
    }
}
