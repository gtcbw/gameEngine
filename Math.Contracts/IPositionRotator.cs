namespace Math.Contracts
{
    public interface IPositionRotator
    {
        double[] Rotate(double x, double y, double z, double rotationXZ);
    }
}
