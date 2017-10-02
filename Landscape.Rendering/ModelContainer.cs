using Engine.Contracts;
using System.Collections.Generic;
using System.Linq;
using Engine.Contracts.Models;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class ModelContainer : IModelContainer, IRenderingElement, IComplexShapeProvider
    {
        private IModelRepository _modelRepository;
        private IModelInstanceRenderer _modelInstanceRenderer;
        private Dictionary<int, List<ModelInstance>> _models = new Dictionary<int, List<ModelInstance>>();
        private List<ComplexShapeInstance> _shapes = new List<ComplexShapeInstance>();

        public ModelContainer(IModelRepository modelRepository,
            IModelInstanceRenderer modelInstanceRenderer)
        {
            _modelRepository = modelRepository;
            _modelInstanceRenderer = modelInstanceRenderer;
        }

        void IModelContainer.AddModel(int fieldId, ModelInstance model)
        {
            if (_models.Keys.Contains(fieldId))
                _models[fieldId].Add(model);
            else
                _models.Add(fieldId, new List<ModelInstance> { model });

            _shapes.Add(model.CollisionModelInstance);
        }

        void IModelContainer.RemoveModels(int fieldId)
        {
            if (!_models.Keys.Contains(fieldId))
                return;

            List<ModelInstance> modelsToRemove = _models[fieldId];

            foreach(ModelInstance model in modelsToRemove)
            {
                _modelRepository.Delete(model);
                _shapes.Remove(model.CollisionModelInstance);
            }

            _models.Remove(fieldId);
        }

        IEnumerable<ComplexShapeInstance> IComplexShapeProvider.GetComplexShapes()
        {
            return _shapes;
        }

        void IRenderingElement.Render()
        {
            foreach(int key in _models.Keys)
            {
                foreach(ModelInstance model in _models[key])
                {
                    _modelInstanceRenderer.Render(model);
                }
            }
        }
    }
}
