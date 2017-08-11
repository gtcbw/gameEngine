namespace Engine.Contracts.Models
{
    public interface IModelQueue
    {
        void QueueModel(int fieldId, ModelInstanceDescription modelInstance);
        void UnqueueNextModel();
        void RemoveModels(int fieldId);
    }
}
