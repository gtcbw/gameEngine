using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Contracts.Models
{
    public sealed class Submodel
    {
        public string Texture { set; get; }

        public List<Polygon> Polygons { set; get; }
    }
}
