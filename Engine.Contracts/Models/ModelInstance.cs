using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts.Models
{
    public sealed class ModelInstance
    {
        public string FileName { set; get; }

        public List<ModelRenderUnit> RenderUnits { set; get; }

        public ComplexShapeInstance CollisionModelInstance { set; get; }
    }
}
