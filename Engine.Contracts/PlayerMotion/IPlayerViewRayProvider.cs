using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IPlayerViewRayProvider
    {
        Ray GetPlayerViewRay();
    }
}
