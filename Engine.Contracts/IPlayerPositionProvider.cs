using World.Model;

namespace Engine.Contracts
{
    public interface IPlayerPositionProvider
    {
        Position GetPlayerPosition();

        double GetHeight();
    }
}
