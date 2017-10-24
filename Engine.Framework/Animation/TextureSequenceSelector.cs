using Engine.Contracts.Animation;

namespace Engine.Framework.Animation
{
    public sealed class TextureSequenceSelector : ITextureSequenceSelector
    {
        SelectedTextureSequence ITextureSequenceSelector.SelectedTextureSequence(TextureSequence360Degree textureSequence360Degree, RotationDegrees rotationDegrees)
        {
            switch (rotationDegrees)
            {
                case RotationDegrees.degree_0:
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_0] };
                case RotationDegrees.degree_45:
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_45] };
                case RotationDegrees.degree_90:
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_90] };
                case RotationDegrees.degree_135:
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_135] };
                case RotationDegrees.degree_180:
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_180] };
                case RotationDegrees.degree_225:
                   if (textureSequence360Degree.TextureSequences.Keys.Contains(RotationDegrees.degree_225))
                        return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_225] };
                   return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_135], IsMirrored = true };
                case RotationDegrees.degree_270:
                    if (textureSequence360Degree.TextureSequences.Keys.Contains(RotationDegrees.degree_270))
                        return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_270] };
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_90], IsMirrored = true };
                case RotationDegrees.degree_315:
                    if (textureSequence360Degree.TextureSequences.Keys.Contains(RotationDegrees.degree_315))
                        return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_315] };
                    return new SelectedTextureSequence { TextureSequence = textureSequence360Degree.TextureSequences[RotationDegrees.degree_45], IsMirrored = true };
            }
            return null;
        }
    }
}
