using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts.Models
{
    public interface ICuboidWithModelsTester
    {
        bool CuboidCollides(IEnumerable<ComplexShapeInstance> models, Cuboid cuboid, Position position);
    }
}
