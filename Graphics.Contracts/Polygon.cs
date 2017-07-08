using System.Collections.Generic;

namespace Graphics.Contracts
{
    public sealed class Polygon
    {
        public Normal Normal { set; get; }

        public IEnumerable<Vertex> Vertices { set; get; }
    }
}
