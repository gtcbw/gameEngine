using Engine.Contracts;
using Engine.Contracts.PlayerMotion;
using Graphics.Contracts;
using System.Collections.Generic;

namespace Landscape.Rendering
{
    public class Horizon : IRenderingElement
    {
        private ITexture _texture;
        private ITextureChanger _textureChanger;
        private IPolygonRenderer _polygonRenderer;
        private IEnumerable<Polygon> _polygons;
        private IPlayerViewDirectionProvider _playerViewDirectionProvider;
        private ITranslator _worldTranslator;
        private readonly IMatrixManager _matrixManager;

        public Horizon(ITexture texture, 
            ITextureChanger textureChanger, 
            IPolygonRenderer polygonRenderer,
            IEnumerable<Polygon> polygons, 
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            ITranslator worldTranslator,
            IMatrixManager matrixManager)
        {
            _texture = texture;
            _textureChanger = textureChanger;
            _polygonRenderer = polygonRenderer;
            _polygons = polygons;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _worldTranslator = worldTranslator;
            _matrixManager = matrixManager;
        }

        void IRenderingElement.Render()
        {
            _textureChanger.SetTexture(_texture.TextureId);
            _matrixManager.Store();
            _worldTranslator.Translate(0, -_playerViewDirectionProvider.GetViewDirection().DegreeY / 90.0, 0);
            _matrixManager.SetTextureMode();
            _matrixManager.Store();
            _worldTranslator.Translate((_playerViewDirectionProvider.GetViewDirection().DegreeXZ / 360.0), 0, 0);
            _polygonRenderer.RenderPolygons(_polygons);
            _matrixManager.Reset();
            _matrixManager.SetModelMode();
            _matrixManager.Reset();
        }
    }
}
