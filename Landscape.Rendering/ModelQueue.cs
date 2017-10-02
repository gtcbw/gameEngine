using Engine.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace Landscape.Rendering
{
    public sealed class ModelQueue : IModelQueue
    {
        private IModelRepository _modelRepository;
        private IModelContainer _modelContainer;
        private Dictionary<int, List<ModelInstanceDescription>> _queuedModels = new Dictionary<int, List<ModelInstanceDescription>>();

        public ModelQueue(IModelRepository modelRepository,
            IModelContainer modelContainer)
        {
            _modelRepository = modelRepository;
            _modelContainer = modelContainer;
        }

        void IModelQueue.QueueModel(int fieldId, ModelInstanceDescription modelInstance)
        {
            if (_queuedModels.Keys.Contains(fieldId))
                _queuedModels[fieldId].Add(modelInstance);
            else
                _queuedModels.Add(fieldId, new List<ModelInstanceDescription> { modelInstance });
        }

        void IModelQueue.RemoveModels(int fieldId)
        {
            if (_queuedModels.Keys.Contains(fieldId))
                _queuedModels.Remove(fieldId);

            _modelContainer.RemoveModels(fieldId);
        }

        void IModelQueue.UnqueueNextModel()
        {
            if (!_queuedModels.Keys.Any())
                return;

            int key = _queuedModels.Keys.First();
            List<ModelInstanceDescription> fieldQueue = _queuedModels[key];
            ModelInstanceDescription modelInstance = fieldQueue.First();

            ModelInstance model = _modelRepository.Load(modelInstance);

            _modelContainer.AddModel(key, model);

            fieldQueue.Remove(modelInstance);

            if (!fieldQueue.Any())
                _queuedModels.Remove(key);
        }
    }
}
