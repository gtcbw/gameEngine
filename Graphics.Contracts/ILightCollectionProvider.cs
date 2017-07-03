namespace Graphics.Contracts
{
    public interface ILightCollectionProvider
    {
        ILightCollection GetCollection(int levelId);
    }
}
