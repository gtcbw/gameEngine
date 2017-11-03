using World.Model;

namespace Math.Contracts
{
    public interface IRotationCalculator
    {
        TwoComponentRotation CalculateRotation(Vector vector);
    }
}
