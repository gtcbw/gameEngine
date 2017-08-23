using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts.Models
{
    public interface IComplexShapeProvider
    {
        IEnumerable<ComplexShapeInstance> GetComplexShapes();
    }
}
