using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using World.Model;

namespace Engine.Framework
{
    public sealed class SpriteRenderer : ISpriteRenderer
    {
        private readonly IRenderingElement _renderingElement;
        private readonly IWorldTranslator _worldTranslator;
        private readonly IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private readonly IWorldRotator _worldRotator;

        public SpriteRenderer(IRenderingElement renderingElement,
            IWorldTranslator worldTranslator,
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            IWorldRotator worldRotator)
        {
            _renderingElement = renderingElement;
            _worldTranslator = worldTranslator;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldRotator = worldRotator;
        }

        void ISpriteRenderer.RenderSpriteAtPosition(IReadOnlyPosition position)
        {
            _worldTranslator.Store();
            _worldTranslator.TranslateWorld(position.X, position.Y, position.Z);
            _worldRotator.RotateY(270 - _playerViewDirectionProvider.GetViewDirection().DegreeXZ);
            _renderingElement.Render();
            _worldTranslator.Reset();
        }
    }
}
