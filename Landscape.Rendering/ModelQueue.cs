using Engine.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landscape.Rendering
{
    public sealed class ModelQueue : IModelQueue
    {
        private IModelRepository _modelRepository;
        private Dictionary<int, List<ModelLocation>> _queuedModels = new Dictionary<int, List<ModelLocation>>();

        public ModelQueue(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        void IModelQueue.QueueModel(int fieldId, ModelLocation modelLocation)
        {
            if (_queuedModels.Keys.Contains(fieldId))
                _queuedModels[fieldId].Add(modelLocation);
            else
                _queuedModels.Add(fieldId, new List<ModelLocation> { modelLocation });
        }

        void IModelQueue.RemoveModels(int fieldId)
        {
            if (_queuedModels.Keys.Contains(fieldId))
                _queuedModels.Remove(fieldId);

            //dfjgfgjfhgjfhfj
        }

        void IModelQueue.UnqueueNextModel()
        {
            if (!_queuedModels.Keys.Any())
                return;

            int key = _queuedModels.Keys.First();
            List<ModelLocation> fieldQueue = _queuedModels[key];
            ModelLocation modelLocation = fieldQueue.First();

            Model model = _modelRepository.Load(modelLocation.Filename);

            //TODO ddgfgfgfg

            fieldQueue.Remove(modelLocation);

            if (!fieldQueue.Any())
                _queuedModels.Remove(key);
        }
    }
}
