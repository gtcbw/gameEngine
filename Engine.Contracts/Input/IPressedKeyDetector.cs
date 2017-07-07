namespace Engine.Contracts.Input
{
    public interface IPressedKeyDetector
    {
        bool IsKeyDown(Keys key);
    }
}
