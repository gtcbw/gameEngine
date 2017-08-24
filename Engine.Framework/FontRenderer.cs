using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public sealed class FontRenderer : IFontRenderer
    {
        private readonly IEnumerable<Polygon> _polygons;
        private readonly IPolygonRenderer _polygonRenderer;
        private readonly IWorldTranslator _worldTranslator;
        private readonly ITextureChanger _textureChanger;
        private readonly double _fontWidth;

        public FontRenderer(IEnumerable<Polygon> polygons,
            IPolygonRenderer polygonRenderer,
            IWorldTranslator worldTranslator,
            ITextureChanger textureChanger,
            double fontWidth)
        {
            _polygons = polygons;
            _polygonRenderer = polygonRenderer;
            _worldTranslator = worldTranslator;
            _textureChanger = textureChanger;
            _fontWidth = fontWidth;
        }

        void IFontRenderer.RenderFont(IEnumerable<int> characterTextures, double startX, double startY)
        {
            _worldTranslator.Store();
            _worldTranslator.TranslateWorld(startX, startY, 0);

            foreach(int texture in characterTextures)
            {
                _textureChanger.SetTexture(texture);
                _polygonRenderer.RenderPolygons(_polygons);
                _worldTranslator.TranslateWorld(_fontWidth, 0, 0);
            }

            _worldTranslator.Reset();
        }
    }
}
