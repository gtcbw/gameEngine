using World.Model;

namespace Math.Contracts
{
    public interface IIntersectionCalculator
    {
        Position RayHitsTriangle(double[] orig, double[] dir, double[] vert0, double[] vert1, double[] vert2);
    }
}
