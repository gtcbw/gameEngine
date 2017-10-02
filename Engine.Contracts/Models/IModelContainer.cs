namespace Engine.Contracts.Models
{
    public interface IModelContainer
    {
        void AddModel(int fieldId, ModelInstance model);

        void RemoveModels(int fieldId);
    }
}
