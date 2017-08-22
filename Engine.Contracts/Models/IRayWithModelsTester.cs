using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts.Models
{
    public interface IRayWithModelsTester
    {
        Position TestRayWithModels(IEnumerable<ComplexShapeInstance> models, Ray ray, double maxDistance);
    }
}
