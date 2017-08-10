using Graphics.Contracts;

namespace Engine.Contracts.Models
{
    public sealed class ModelRenderUnit
    {
        public VertexBufferUnit VertexBufferUnit { set; get; }

        public ITexture Texture { set; get; }
    }
}
