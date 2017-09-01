using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts.Models
{
    public sealed class EditorModel
    {
        public List<Submodel> Submodels { set; get; }

        public ComplexShape CollisionModel { set; get; }
    }
}
