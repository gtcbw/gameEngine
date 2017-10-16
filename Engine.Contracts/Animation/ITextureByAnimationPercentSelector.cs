namespace Engine.Contracts.Animation
{
    public interface ITextureByAnimationPercentSelector
    {
        int GetTextureIdByPercentage(TextureSequence textureSequence, double percentage);
    }
}
