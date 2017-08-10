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
        void IModelQueue.QueueModel(int fieldId, ModelLocation modelLocation)
        {
            throw new NotImplementedException();
        }

        void IModelQueue.RemoveModels(int fieldId)
        {
            throw new NotImplementedException();
        }

        void IModelQueue.UnqueueNextModel()
        {
            throw new NotImplementedException();
        }
    }
}
