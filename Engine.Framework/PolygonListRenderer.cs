using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public sealed class PolygonListRenderer : IRenderingElement
    {
        private readonly IEnumerable<Polygon> polygons;
        private readonly IPolygonRenderer polygonRenderer;

        public PolygonListRenderer(IEnumerable<Polygon> polygons, IPolygonRenderer polygonRenderer)
        {
            this.polygons = polygons;
            this.polygonRenderer = polygonRenderer;
        }

        void IRenderingElement.Render()
        {
            polygonRenderer.RenderPolygons(polygons);
        }
    }
}
