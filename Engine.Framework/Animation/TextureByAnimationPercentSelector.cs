using Engine.Contracts.Animation;
using System.Linq;

namespace Engine.Framework.Animation
{
    public sealed class TextureByAnimationPercentSelector : ITextureByAnimationPercentSelector
    {
        int ITextureByAnimationPercentSelector.GetTextureIdByPercentage(TextureSequence textureSequence, double percentage)
        {
            double percentageStep = 1.0 / textureSequence.Textures.Count();

            double addedStep = percentageStep;
            int index = 0;

            while (addedStep <= 1.0)
            {
                if (percentage < addedStep)
                    return textureSequence.Textures[index].TextureId;
                addedStep += percentageStep;
                index++;
            }

            return textureSequence.Textures[textureSequence.Textures.Count() - 1].TextureId;
        }
    }
}
