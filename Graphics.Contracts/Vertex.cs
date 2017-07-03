using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public class Vertex
    {
        public VertexPosition Position { set; get; }

        public VertexTextureCoordinate TextureCoordinate { set; get; }
    }
}
