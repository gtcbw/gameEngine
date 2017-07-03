using System.Collections.Generic;

namespace Graphics.Contracts
{
    public interface IPolygonRenderer
    {
        void RenderPolygons(List<Polygon> polygons);
    }
}
