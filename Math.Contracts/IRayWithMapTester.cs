using World.Model;

namespace Math.Contracts
{
    public interface IRayWithMapTester
    {
        Position FindCollisionWithMap(Ray ray);
    }
}
