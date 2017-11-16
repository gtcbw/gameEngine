using System;
using Engine.Contracts;
using Engine.Contracts.Animation;
using Engine.Contracts.Models;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using Math.Contracts;
using World.Model;

namespace Character.Animation
{
    public sealed class Animation360DegreeRenderer : IRenderingElement
    {
        private readonly ITextureByAnimationPercentSelector _textureByAnimationPercentSelector;
        private readonly ITextureSequenceSelector _textureSequenceSelector;
        private readonly ITextureChanger _textureChanger;
        private readonly IPercentProvider _percentProvider;
        private readonly TextureSequence360Degree _walkAnimation;
        private readonly TextureSequence360Degree _torso;
        private readonly TextureSequence360Degree _head;

        private readonly ModelInstance _modelInstance;
        private readonly IModelInstanceRenderer _modelInstanceRenderer;

        private readonly IRenderedRotationCalculator _renderedRotationCalculator;
        private readonly IMatrixManager _matrixManager;
        private readonly IRenderingElement _footSprite;
        private readonly IRenderingElement _torsoSprite;
        private readonly IRenderingElement _headSprite;
        private readonly ITranslator _worldTranslator;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IWorldRotator _worldRotator;

        private readonly IFrameTimeProvider _timeProvider;
        private readonly IHeightCalculator _heightCalculator;
        private readonly IPlayerPositionProvider _playerPositionProvider;
        private readonly IRotationCalculator _rotationCalculator;
        private readonly Position _position = new Position { X = 105, Y = 1, Z = 106 };

        public Animation360DegreeRenderer(ITextureByAnimationPercentSelector textureByAnimationPercentSelector,
            ITextureSequenceSelector textureSequenceSelector,
            ITextureChanger textureChanger,
            IPercentProvider percentProvider,
            TextureSequence360Degree walkAnimation,
            TextureSequence360Degree torso,
            TextureSequence360Degree head,
            ModelInstance modelInstance,
            IModelInstanceRenderer modelInstanceRenderer,
            IRenderedRotationCalculator renderedRotationCalculator,
            IMatrixManager matrixManager,
            IRenderingElement footSprite,
            IRenderingElement torsoSprite,
            IRenderingElement headSprite,
            ITranslator worldTranslator,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IWorldRotator worldRotator,

            IFrameTimeProvider timeProvider, 
            IHeightCalculator heightCalculator,
            
            IPlayerPositionProvider playerPositionProvider,
            IRotationCalculator rotationCalculator)
        {
            _textureByAnimationPercentSelector = textureByAnimationPercentSelector;
            _textureSequenceSelector = textureSequenceSelector;
            _textureChanger = textureChanger;
            _percentProvider = percentProvider;
            _walkAnimation = walkAnimation;
            _torso = torso;
            _head = head;
            _modelInstance = modelInstance;
            _modelInstanceRenderer = modelInstanceRenderer;
            _renderedRotationCalculator = renderedRotationCalculator;
            _matrixManager = matrixManager;
            _footSprite = footSprite;
            _torsoSprite = torsoSprite;
            _headSprite = headSprite;
            _worldTranslator = worldTranslator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldRotator = worldRotator;

            _timeProvider = timeProvider;
            _heightCalculator = heightCalculator;
            _playerPositionProvider = playerPositionProvider;
            _rotationCalculator = rotationCalculator;
        }

        void IRenderingElement.Render()
        {
            if (_percentProvider.IsOver())
                _percentProvider.Start();
            RotationDegrees rotationDegrees = RotationDegrees.degree_0;
            double percent = _percentProvider.GetPercent();

            //_position.Z -= _timeProvider.GetTimeInSecondsSinceLastFrame() * 1.8;
            _position.X += _timeProvider.GetTimeInSecondsSinceLastFrame() * 1.8;
            _position.Y = _heightCalculator.CalculateHeight(_position.X, _position.Z);

            var renderedRotation = _renderedRotationCalculator.CalculateRotationRelativeToCamera(rotationDegrees, _position.X, _position.Z);

            SelectedTextureSequence selectedWalkAnimationTexture = _textureSequenceSelector.SelectedTextureSequence(_walkAnimation, renderedRotation);
            int footTextureId = _textureByAnimationPercentSelector.GetTextureIdByPercentage(selectedWalkAnimationTexture.TextureSequence, percent);

            SelectedTextureSequence selectedTorsoTexture = _textureSequenceSelector.SelectedTextureSequence(_torso, renderedRotation);
            int torsoTextureId = selectedTorsoTexture.TextureSequence.Textures[0].TextureId;

            SelectedTextureSequence selectedHeadTexture = _textureSequenceSelector.SelectedTextureSequence(_head, renderedRotation);
            int headTextureId = selectedHeadTexture.TextureSequence.Textures[0].TextureId;


            double torsoY = (System.Math.Sin((percent * 4 - 0.5) * System.Math.PI) + 1) * 0.5;
            //double torsoY = (System.Math.Sin((percent * 2 - 1.5) * System.Math.PI) + 1) * 0.2 + 0.7;

            double gunrotation = CalculateGunRotation();

            _matrixManager.Store();
            _worldTranslator.Translate(_position.X, _position.Y, _position.Z);
            _matrixManager.Store();
            _worldRotator.RotateY((selectedWalkAnimationTexture.IsMirrored ? 90 : 270) - _playerViewDirectionProvider.GetViewDirection().DegreeXZ);
            _textureChanger.SetTexture(footTextureId);
            _footSprite.Render();

            _worldTranslator.Translate(0, 0.3 + torsoY * 0.15, 0);
            _textureChanger.SetTexture(torsoTextureId);
            _torsoSprite.Render();

            _worldTranslator.Translate(0, 0.75 + torsoY * 0.05, 0);
            _textureChanger.SetTexture(headTextureId);
            _headSprite.Render();

            _matrixManager.Reset();
            _worldTranslator.Translate(0, 0.1 + torsoY * 0.15, 0.7);
            _matrixManager.Store();
            
            _worldRotator.RotateY(gunrotation);
            //_worldRotator.RotateZ(20);
            _modelInstanceRenderer.Render(_modelInstance);
            _matrixManager.Reset();

            _worldTranslator.Translate(0, 0, -1.4);
            //_worldRotator.RotateZ(20);
            _worldRotator.RotateY(gunrotation);
            _modelInstanceRenderer.Render(_modelInstance);

            _matrixManager.Reset();
        }

        private double CalculateGunRotation()
        {
            var playerPosition = _playerPositionProvider.GetPlayerPosition();

            Vector vector = new Vector
            {
                X = playerPosition.X - _position.X,
                Y = playerPosition.Y - _position.Y,
                Z = playerPosition.Z - _position.Z
            };

            var rotation = _rotationCalculator.CalculateRotation(vector);

            return rotation.DegreeXZ;
        }
    }
}
