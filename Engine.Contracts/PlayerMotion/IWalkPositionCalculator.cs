using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IWalkPositionCalculator
    {
        WalkMotion CalculateNextPosition(WalkMotion lastWalkMotion);
    }
}
