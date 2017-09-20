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
        private ITextureTranslator _textureTranslator;
        private IWorldTranslator _worldTranslator;

        public Horizon(ITexture texture, 
            ITextureChanger textureChanger, 
            IPolygonRenderer polygonRenderer,
            IEnumerable<Polygon> polygons, 
            IPlayerViewDirectionProvider playerViewDirectionProvider,
            ITextureTranslator textureTranslator,
            IWorldTranslator worldTranslator)
        {
            _texture = texture;
            _textureChanger = textureChanger;
            _polygonRenderer = polygonRenderer;
            _polygons = polygons;
            _playerViewDirectionProvider = playerViewDirectionProvider;
            _textureTranslator = textureTranslator;
            _worldTranslator = worldTranslator;
        }

        void IRenderingElement.Render()
        {
            _textureChanger.SetTexture(_texture.TextureId);
            _worldTranslator.Store();
            _worldTranslator.TranslateWorld(0, -_playerViewDirectionProvider.GetViewDirection().DegreeY / 90.0, 0);
            _textureTranslator.Store();
            _textureTranslator.TranslateTexture((_playerViewDirectionProvider.GetViewDirection().DegreeXZ / 360.0), 0);
            _polygonRenderer.RenderPolygons(_polygons);
            _textureTranslator.Reset();
            _worldTranslator.Reset();
        }
    }
}
