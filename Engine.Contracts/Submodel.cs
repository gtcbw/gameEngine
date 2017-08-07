using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Contracts
{
    public sealed class Submodel
    {
        public string Texture { set; get; }

        public List<Polygon> Polygons { set; get; }
    }
}
