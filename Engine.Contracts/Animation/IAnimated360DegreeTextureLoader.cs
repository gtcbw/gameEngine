namespace Engine.Contracts.Animation
{
    public interface IAnimated360DegreeTextureLoader
    {
        TextureSequence360Degree LoadAnimatedTexture(string animationName);
    }
}
