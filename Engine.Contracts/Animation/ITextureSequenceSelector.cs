namespace Engine.Contracts.Animation
{
    public interface ITextureSequenceSelector
    {
        SelectedTextureSequence SelectedTextureSequence(TextureSequence360Degree textureSequence360Degree, RotationDegrees rotationDegrees);
    }
}
