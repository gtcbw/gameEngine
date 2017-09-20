using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IPlayerMotionEncapsulator : IPlayerPositionProvider, IPlayerViewRayProvider, IPlayerViewDirectionProvider
    {
        void SetMotion(Position position, double height, double degreeXZ, double degreeY, Vector2D vectorXZ = null);
    }
}
