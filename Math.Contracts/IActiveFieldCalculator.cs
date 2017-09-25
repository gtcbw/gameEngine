using System.Collections.Generic;
using World.Model;

namespace Math.Contracts
{
    public interface IActiveFieldCalculator
    {
        IEnumerable<FieldCoordinates> CalculateActiveFields(IReadOnlyPosition position);
    }
}
