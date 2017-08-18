using World.Model;

namespace Math.Contracts
{
    public interface IRayWithFacesTester
    {
        Position SearchCollision(double[] rayStartPosition, double[] rayDirection, Face[] faces);
    }
}
