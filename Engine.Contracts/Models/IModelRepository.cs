namespace Engine.Contracts.Models
{
    public interface IModelRepository
    {
        Model Load(string filename);

        void Delete(Model model);
    }
}
