using Engine.Contracts.Models;
using Graphics.Contracts;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class ModelInstanceRenderer : IModelInstanceRenderer
    {
        private ITextureChanger _textureChanger;
        private IVertexBufferUnitRenderer _vertexBufferUnitRenderer;
        private IWorldTranslator _worldTranslator;
        private readonly IWorldRotator _worldRotator;

        public ModelInstanceRenderer(ITextureChanger textureChanger,
            IVertexBufferUnitRenderer vertexBufferUnitRenderer,
            IWorldTranslator worldTranslator,
            IWorldRotator worldRotator)
        {
            _textureChanger = textureChanger;
            _vertexBufferUnitRenderer = vertexBufferUnitRenderer;
            _worldTranslator = worldTranslator;
            _worldRotator = worldRotator;
        }

        void IModelInstanceRenderer.Render(ModelInstance model)
        {
            _worldTranslator.Store();
            Position position = model.CollisionModelInstance.Position;
            _worldTranslator.TranslateWorld(position.X, position.Y, position.Z);
            _worldRotator.RotateY(model.CollisionModelInstance.RotationXZ);

            foreach (ModelRenderUnit unit in model.RenderUnits)
            {
                _textureChanger.SetTexture(unit.Texture.TextureId);
                _vertexBufferUnitRenderer.RenderMesh(unit.VertexBufferUnit);
            }
            _worldTranslator.Reset();
        }
    }
}
