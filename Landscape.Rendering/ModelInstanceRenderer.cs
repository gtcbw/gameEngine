using Engine.Contracts.Models;
using Graphics.Contracts;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class ModelInstanceRenderer : IModelInstanceRenderer
    {
        private ITextureChanger _textureChanger;
        private IVertexBufferUnitRenderer _vertexBufferUnitRenderer;
        private ITranslator _worldTranslator;
        private readonly IWorldRotator _worldRotator;
        private readonly IMatrixManager _matrixManager;

        public ModelInstanceRenderer(ITextureChanger textureChanger,
            IVertexBufferUnitRenderer vertexBufferUnitRenderer,
            ITranslator worldTranslator,
            IWorldRotator worldRotator,
            IMatrixManager matrixManager)
        {
            _textureChanger = textureChanger;
            _vertexBufferUnitRenderer = vertexBufferUnitRenderer;
            _worldTranslator = worldTranslator;
            _worldRotator = worldRotator;
            _matrixManager = matrixManager;
        }

        void IModelInstanceRenderer.Render(ModelInstance model)
        {
            _matrixManager.Store();
            Position position = model.CollisionModelInstance.Position;
            _worldTranslator.Translate(position.X, position.Y, position.Z);
            _worldRotator.RotateY(model.CollisionModelInstance.RotationXZ);

            foreach (ModelRenderUnit unit in model.RenderUnits)
            {
                _textureChanger.SetTexture(unit.Texture.TextureId);
                _vertexBufferUnitRenderer.RenderMesh(unit.VertexBufferUnit);
            }
            _matrixManager.Reset();
        }
    }
}
