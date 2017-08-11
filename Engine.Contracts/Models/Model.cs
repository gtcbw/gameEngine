using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts.Models
{
    public sealed class Model
    {
        public Position Position { set; get; }

        public string FileName { set; get; }

        public List<ModelRenderUnit> RenderUnits { set; get; }
    }
}
