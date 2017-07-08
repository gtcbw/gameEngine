using System.Collections.Generic;

namespace Graphics.Contracts
{
    public interface IPolygonRenderer
    {
        void RenderPolygons(IEnumerable<Polygon> polygons);
    }
}
