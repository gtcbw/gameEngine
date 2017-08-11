using Engine.Contracts;
using Engine.Contracts.Models;
using System.Collections.Generic;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class ModelQueuePusher : IFieldChangeObserver
    {
        private IFieldModelLoader _fieldModelLoader;
        private IModelQueue _modelQueue;

        public ModelQueuePusher(IFieldModelLoader fieldModelLoader,
            IModelQueue modelQueue)
        {
            _fieldModelLoader = fieldModelLoader;
            _modelQueue = modelQueue;
        }

        void IFieldChangeObserver.NotifyChangedFields(IEnumerable<FieldCoordinates> addedFields, 
            IEnumerable<FieldCoordinates> removedFields)
        {
            foreach (FieldCoordinates field in addedFields)
            {
                var modelList = _fieldModelLoader.LoadModelsForField(field.Z, field.X);

                foreach (ModelInstanceDescription modelInstance in modelList)
                {
                    _modelQueue.QueueModel(field.ID, modelInstance);
                }
            }

            foreach (FieldCoordinates field in removedFields)
            {
                _modelQueue.RemoveModels(field.ID);
            }

            _modelQueue.UnqueueNextModel();
        }
    }
}
