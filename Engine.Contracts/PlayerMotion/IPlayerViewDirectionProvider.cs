using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IPlayerViewDirectionProvider
    {
        TwoComponentRotation GetViewDirection();
    }
}
