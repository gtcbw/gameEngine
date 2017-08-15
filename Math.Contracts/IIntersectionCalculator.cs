using World.Model;

namespace Math.Contracts
{
    public interface IIntersectionCalculator
    {
        Position RayHitsTriangle(Ray ray, Position corner1, Position corner2, Position corner3);
    }
}
