using Engine.Contracts;
using Engine.Contracts.Animation;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using World.Model;

namespace Engine.Framework.Animation
{
    public sealed class Animation360DegreeRenderer : IRenderingElement
    {
        private readonly ITextureByAnimationPercentSelector _textureByAnimationPercentSelector;
        private readonly ITextureSequenceSelector _textureSequenceSelector;
        private readonly ITextureChanger _textureChanger;
        private readonly IPercentProvider _percentProvider;
        private readonly TextureSequence360Degree _walkAnimation;
        private readonly IRenderedRotationCalculator _renderedRotationCalculator;
        private readonly IMatrixManager _matrixManager;
        private readonly IRenderingElement _footSprite;
        private readonly ITranslator _worldTranslator;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IWorldRotator _worldRotator;

        public Animation360DegreeRenderer(ITextureByAnimationPercentSelector textureByAnimationPercentSelector,
            ITextureSequenceSelector textureSequenceSelector,
            ITextureChanger textureChanger,
            IPercentProvider percentProvider,
            TextureSequence360Degree walkAnimation,
            IRenderedRotationCalculator renderedRotationCalculator,
            IMatrixManager matrixManager,
            IRenderingElement footSprite,
            ITranslator worldTranslator,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IWorldRotator worldRotator)
        {
            _textureByAnimationPercentSelector = textureByAnimationPercentSelector;
            _textureSequenceSelector = textureSequenceSelector;
            _textureChanger = textureChanger;
            _percentProvider = percentProvider;
            _walkAnimation = walkAnimation;
            _renderedRotationCalculator = renderedRotationCalculator;
            _matrixManager = matrixManager;
            _footSprite = footSprite;
            _worldTranslator = worldTranslator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldRotator = worldRotator;
        }

        void IRenderingElement.Render()
        {
            if (_percentProvider.IsOver())
                _percentProvider.Start();
            RotationDegrees rotationDegrees = RotationDegrees.degree_180;
            double percent = _percentProvider.GetPercent();

            IReadOnlyPosition position = new Position { X = 110, Y = 1, Z = 110 };

            var renderedRotation = _renderedRotationCalculator.CalculateRotationRelativeToCamera(rotationDegrees, position.X, position.Z);

            SelectedTextureSequence selectedTextureSequence = _textureSequenceSelector.SelectedTextureSequence(_walkAnimation, renderedRotation);
            int textureId = _textureByAnimationPercentSelector.GetTextureIdByPercentage(selectedTextureSequence.TextureSequence, percent);
            _textureChanger.SetTexture(textureId);

            _matrixManager.Store();
            _worldTranslator.Translate(position.X, position.Y, position.Z);
            _worldRotator.RotateY((selectedTextureSequence.IsMirrored ? 90 : 270) - _playerViewDirectionProvider.GetViewDirection().DegreeXZ);
            _footSprite.Render();
            _matrixManager.Reset();
        }
    }
}
