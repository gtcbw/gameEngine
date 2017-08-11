using Engine.Contracts;
using System.Collections.Generic;
using System.Linq;
using Engine.Contracts.Models;
using Graphics.Contracts;

namespace Landscape.Rendering
{
    public sealed class ModelContainer : IModelContainer, IRenderingElement
    {
        private IModelRepository _modelRepository;
        private ITextureChanger _textureChanger;
        private IVertexBufferUnitRenderer _vertexBufferUnitRenderer;
        private IWorldTranslator _worldTranslator;
        private Dictionary<int, List<Model>> _models = new Dictionary<int, List<Model>>();

        public ModelContainer(IModelRepository modelRepository,
            ITextureChanger textureChanger,
            IVertexBufferUnitRenderer vertexBufferUnitRenderer,
            IWorldTranslator worldTranslator)
        {
            _modelRepository = modelRepository;
            _textureChanger = textureChanger;
            _vertexBufferUnitRenderer = vertexBufferUnitRenderer;
            _worldTranslator = worldTranslator;
        }

        void IModelContainer.AddModel(int fieldId, Model model)
        {
            if (_models.Keys.Contains(fieldId))
                _models[fieldId].Add(model);
            else
                _models.Add(fieldId, new List<Model> { model });
        }

        void IModelContainer.RemoveModels(int fieldId)
        {
            if (!_models.Keys.Contains(fieldId))
                return;

            List<Model> modelsToRemove = _models[fieldId];

            foreach(Model model in modelsToRemove)
            {
                _modelRepository.Delete(model);
            }

            _models.Remove(fieldId);
        }

        void IRenderingElement.Render()
        {
            foreach(int key in _models.Keys)
            {
                foreach(Model model in _models[key])
                {
                    _worldTranslator.Store();
                    _worldTranslator.TranslateWorld(model.Position.X, model.Position.Y, model.Position.Z);

                    foreach (ModelRenderUnit unit in model.RenderUnits)
                    {
                        _textureChanger.SetTexture(unit.Texture.TextureId);
                        _vertexBufferUnitRenderer.RenderMesh(unit.VertexBufferUnit);
                    }
                    _worldTranslator.Reset();
                }
            }
        }
    }
}
