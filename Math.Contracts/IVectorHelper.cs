using World.Model;

namespace Math.Contracts
{
    public interface IVectorHelper
    {
        Vector CalculateUnitLengthVector(Vector vector);
        Vector CreateFromDegrees(double degreeXZ, double degreeY);
    }
}
