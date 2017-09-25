using World.Model;

namespace Engine.Contracts
{
    public interface ISpriteRenderer
    {
        void RenderSpriteAtPosition(IReadOnlyPosition position);
    }
}
