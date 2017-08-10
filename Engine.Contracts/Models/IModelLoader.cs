namespace Engine.Contracts.Models
{
    public interface IModelLoader
    {
        Model LoadModel(string filename);
    }
}
