using System.Collections.Generic;
using World.Model;

namespace Math.Contracts
{
    public interface IActiveFieldCalculator
    {
        IEnumerable<int> CalculateActiveFields(Position position);
    }
}
