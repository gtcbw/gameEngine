using Engine.Contracts.Models;

namespace Engine.Contracts
{
    public interface IModelContainer
    {
        void AddModel(int fieldId, Model model);

        void RemoveModels(int fieldId);
    }
}
