namespace Engine.Contracts.Models
{
    public interface IModelRepository
    {
        ModelInstance Load(ModelInstanceDescription modelInstance);

        void Delete(ModelInstance model);
    }
}
