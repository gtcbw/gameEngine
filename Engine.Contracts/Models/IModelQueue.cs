namespace Engine.Contracts.Models
{
    public interface IModelQueue
    {
        void QueueModel(int fieldId, ModelLocation modelLocation);
        void UnqueueNextModel();
        void RemoveModels(int fieldId);
    }
}
