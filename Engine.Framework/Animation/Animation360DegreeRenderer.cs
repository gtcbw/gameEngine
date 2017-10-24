using Engine.Contracts;
using Engine.Contracts.Animation;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using Math.Contracts;
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
        private readonly TextureSequence360Degree _torso;
        private readonly IRenderedRotationCalculator _renderedRotationCalculator;
        private readonly IMatrixManager _matrixManager;
        private readonly IRenderingElement _footSprite;
        private readonly IRenderingElement _torsoSprite;
        private readonly ITranslator _worldTranslator;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IWorldRotator _worldRotator;

        private readonly IFrameTimeProvider _timeProvider;
        private readonly IHeightCalculator _heightCalculator;
        private readonly Position _position = new Position { X = 110, Y = 1, Z = 110 };

        public Animation360DegreeRenderer(ITextureByAnimationPercentSelector textureByAnimationPercentSelector,
            ITextureSequenceSelector textureSequenceSelector,
            ITextureChanger textureChanger,
            IPercentProvider percentProvider,
            TextureSequence360Degree walkAnimation,
            TextureSequence360Degree torso,
            IRenderedRotationCalculator renderedRotationCalculator,
            IMatrixManager matrixManager,
            IRenderingElement footSprite,
            IRenderingElement torsoSprite,
            ITranslator worldTranslator,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IWorldRotator worldRotator,

            IFrameTimeProvider timeProvider, 
            IHeightCalculator heightCalculator)
        {
            _textureByAnimationPercentSelector = textureByAnimationPercentSelector;
            _textureSequenceSelector = textureSequenceSelector;
            _textureChanger = textureChanger;
            _percentProvider = percentProvider;
            _walkAnimation = walkAnimation;
            _torso = torso;
            _renderedRotationCalculator = renderedRotationCalculator;
            _matrixManager = matrixManager;
            _footSprite = footSprite;
            _torsoSprite = torsoSprite;
            _worldTranslator = worldTranslator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldRotator = worldRotator;

            _timeProvider = timeProvider;
            _heightCalculator = heightCalculator;
        }

        void IRenderingElement.Render()
        {
            if (_percentProvider.IsOver())
                _percentProvider.Start();
            RotationDegrees rotationDegrees = RotationDegrees.degree_0;
            double percent = _percentProvider.GetPercent();

            _position.X += _timeProvider.GetTimeInSecondsSinceLastFrame() * 1.8;
            _position.Y = _heightCalculator.CalculateHeight(_position.X, _position.Z);

            var renderedRotation = _renderedRotationCalculator.CalculateRotationRelativeToCamera(rotationDegrees, _position.X, _position.Z);

            SelectedTextureSequence selectedWalkAnimationTexture = _textureSequenceSelector.SelectedTextureSequence(_walkAnimation, renderedRotation);
            int footTextureId = _textureByAnimationPercentSelector.GetTextureIdByPercentage(selectedWalkAnimationTexture.TextureSequence, percent);

            SelectedTextureSequence selectedTorsoTexture = _textureSequenceSelector.SelectedTextureSequence(_torso, renderedRotation);
            int torsoTextureId = selectedTorsoTexture.TextureSequence.Textures[0].TextureId;

            double torsoY = (System.Math.Sin((percent * 4 - 0.5) * System.Math.PI) + 1) * 0.5;

            _matrixManager.Store();
            _worldTranslator.Translate(_position.X, _position.Y, _position.Z);
            _worldRotator.RotateY((selectedWalkAnimationTexture.IsMirrored ? 90 : 270) - _playerViewDirectionProvider.GetViewDirection().DegreeXZ);
            _textureChanger.SetTexture(footTextureId);
            _footSprite.Render();

            _worldTranslator.Translate(0, 0.15 + torsoY * 0.15, 0);
            _textureChanger.SetTexture(torsoTextureId);
            _torsoSprite.Render();

            _matrixManager.Reset();
        }
    }
}
