namespace Engine.Contracts.Models
{
    public interface IModelLoader
    {
        Model Load(string fileName);

        void Delete(Model model);
    }
}
