using Engine.Contracts;
using Engine.Contracts.Animation;
using Graphics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Framework.Animation
{
    public sealed class Animation360DegreeRenderer
    {
        private readonly ISpriteRenderer _spriteRenderer;
        private readonly ITextureByAnimationPercentSelector _textureByAnimationPercentSelector;
        private readonly ITextureSequenceSelector _textureSequenceSelector;
        private readonly ITextureChanger _textureChanger;

        public Animation360DegreeRenderer(ISpriteRenderer spriteRenderer,
            ITextureByAnimationPercentSelector textureByAnimationPercentSelector,
            ITextureSequenceSelector textureSequenceSelector,
            ITextureChanger textureChanger)
        {
            _spriteRenderer = spriteRenderer;
            _textureByAnimationPercentSelector = textureByAnimationPercentSelector;
            _textureSequenceSelector = textureSequenceSelector;
            _textureChanger = textureChanger;
        }

        public void Render(TextureSequence360Degree textureSequence360Degree)//, RotationDegrees rotationDegrees, double percent, IReadOnlyPosition position)
        {
            RotationDegrees rotationDegrees = RotationDegrees.degree_0;
            double percent = 0;
            IReadOnlyPosition position = new Position { X = 110, Y = 1, Z = 110 };
            SelectedTextureSequence selectedTextureSequence = _textureSequenceSelector.SelectedTextureSequence(textureSequence360Degree, rotationDegrees);
            int textureId = _textureByAnimationPercentSelector.GetTextureIdByPercentage(selectedTextureSequence.TextureSequence, percent);
            _textureChanger.SetTexture(textureId);
            _spriteRenderer.RenderSpriteAtPosition(position);
        }
    }
}
