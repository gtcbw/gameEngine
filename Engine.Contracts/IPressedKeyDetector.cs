namespace Engine.Contracts
{
    public interface IPressedKeyDetector
    {
        bool IsKeyDown(Keys key);
    }
}
