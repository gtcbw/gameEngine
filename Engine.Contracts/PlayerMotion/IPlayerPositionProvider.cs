using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IPlayerPositionProvider
    {
        IReadOnlyPosition GetPlayerPosition();
    }
}
