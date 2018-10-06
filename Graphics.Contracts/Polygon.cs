using System.Collections.Generic;

namespace Graphics.Contracts
{
    public sealed class Polygon
    {
        public IEnumerable<Vertex> Vertices { set; get; }
    }
}
