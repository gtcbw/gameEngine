using System.Collections.Generic;

namespace Graphics.Contracts
{
    public class Polygon
    {
        public Normal Normal { set; get; }

        public List<Vertex> Vertices { set; get; }
    }
}
