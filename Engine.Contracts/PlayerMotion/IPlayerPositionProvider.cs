using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IPlayerPositionProvider
    {
        Position GetPlayerPosition();
    }
}
