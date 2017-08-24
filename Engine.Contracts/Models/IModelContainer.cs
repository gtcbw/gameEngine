namespace Engine.Contracts.Models
{
    public interface IModelContainer
    {
        void AddModel(int fieldId, Model model);

        void RemoveModels(int fieldId);
    }
}
