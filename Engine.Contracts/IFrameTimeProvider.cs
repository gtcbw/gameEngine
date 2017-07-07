namespace Engine.Contracts
{
    public interface IFrameTimeProvider
    {
        double GetTimeInSecondsSinceLastFrame();
    }
}
