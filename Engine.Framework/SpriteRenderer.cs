using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using World.Model;

namespace Engine.Framework
{
    public sealed class SpriteRenderer : ISpriteRenderer
    {
        private readonly IRenderingElement _renderingElement;
        private readonly ITranslator _worldTranslator;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IWorldRotator _worldRotator;
        private readonly IMatrixManager _matrixManager;

        public SpriteRenderer(IRenderingElement renderingElement,
            ITranslator worldTranslator,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IWorldRotator worldRotator,
            IMatrixManager matrixManager)
        {
            _renderingElement = renderingElement;
            _worldTranslator = worldTranslator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldRotator = worldRotator;
            _matrixManager = matrixManager;
        }

        void ISpriteRenderer.RenderSpriteAtPosition(IReadOnlyPosition position)
        {
            _matrixManager.Store();
            _worldTranslator.Translate(position.X, position.Y, position.Z);
            _worldRotator.RotateY(270 - _playerViewDirectionProvider.GetViewDirection().DegreeXZ);
            _renderingElement.Render();
            _matrixManager.Reset();
        }
    }
}
