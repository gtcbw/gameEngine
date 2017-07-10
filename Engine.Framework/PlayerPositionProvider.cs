using Engine.Contracts;
using World.Model;

namespace Engine.Framework
{
    public class PlayerPositionProvider : IPlayerPositionProvider
    {
        double IPlayerPositionProvider.GetHeight()
        {
            return 1.8;
        }

        Position IPlayerPositionProvider.GetPlayerPosition()
        {
            return new Position();
        }
    }
}
