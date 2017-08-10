using System.Collections.Generic;

namespace Engine.Contracts.Models
{
    public sealed class Model
    {
        public string FileName { set; get; }

        public List<ModelRenderUnit> RenderUnits { set; get; }
    }
}
