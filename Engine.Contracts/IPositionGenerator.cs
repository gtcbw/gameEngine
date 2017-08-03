using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts
{
    public interface IPositionGenerator
    {
        IEnumerable<Position> GeneratePositions(FieldCoordinates field);
    }
}
