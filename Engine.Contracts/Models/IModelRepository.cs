namespace Engine.Contracts.Models
{
    public interface IModelRepository
    {
        Model Load(ModelInstanceDescription modelInstance);

        void Delete(Model model);
    }
}
