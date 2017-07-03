using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public class Polygon
    {
        public Normal Normal { set; get; }

        public List<Vertex> Vertices { set; get; }
    }
}
