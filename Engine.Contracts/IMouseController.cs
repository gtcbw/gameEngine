namespace Engine.Contracts
{
    public interface IMouseController
    {
        MouseEvents GetMouseEvents();
        void ResetPosition();
    }
}
