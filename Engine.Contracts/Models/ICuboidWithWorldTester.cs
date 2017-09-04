using World.Model;

namespace Engine.Contracts.Models
{
    public interface ICuboidWithWorldTester
    {
        bool ElementCollidesWithWorld(Position position, double sideLength, double height);
    }
}
