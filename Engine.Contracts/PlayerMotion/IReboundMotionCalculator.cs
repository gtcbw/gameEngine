namespace Engine.Contracts.PlayerMotion
{
    public interface IReboundMotionCalculator
    {
        ReboundMotion CalculateNextReboundMotion(ReboundMotion currentReboundMotion);
    }
}
