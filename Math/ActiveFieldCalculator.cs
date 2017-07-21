using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Math
{
    public sealed class ActiveFieldCalculator : IActiveFieldCalculator
    {
        private readonly int _lengthPerSquad;
        private readonly int _numberOfSquadsPerSide;

        public ActiveFieldCalculator(int lengthPerSquad, int numberOfSquadsPerSide)
        {
            _lengthPerSquad = lengthPerSquad;
            _numberOfSquadsPerSide = numberOfSquadsPerSide;
        }

        public IEnumerable<FieldCoordinates> CalculateActiveFields(Position position)
        {
            int x = (int)position.X / _lengthPerSquad;
            int z = (int)position.Z / _lengthPerSquad;

            List<FieldCoordinates> result = new List<FieldCoordinates>();

            result.Add(new FieldCoordinates { X = x, Z = z, RelativeID = 4 });

            if (x > 0)
                result.Add(new FieldCoordinates { X = x - 1, Z = z, RelativeID = 3 });
            if (x < _numberOfSquadsPerSide - 1)
                result.Add(new FieldCoordinates { X = x + 1, Z = z, RelativeID = 5 });

            if (z > 0)
            {
                result.Add(new FieldCoordinates { X = x, Z = z - 1, RelativeID = 1 });
                if (x > 0)
                    result.Add(new FieldCoordinates { X = x - 1, Z = z - 1, RelativeID = 0 });
                if (x < _numberOfSquadsPerSide - 1)
                    result.Add(new FieldCoordinates { X = x + 1, Z = z - 1, RelativeID = 2 });
            }

            if (z < _numberOfSquadsPerSide - 1)
            {
                result.Add(new FieldCoordinates { X = x, Z = z + 1, RelativeID = 7 });
                if (x > 0)
                    result.Add(new FieldCoordinates { X = x - 1, Z = z + 1, RelativeID = 6 });
                if (x < _numberOfSquadsPerSide - 1)
                    result.Add(new FieldCoordinates { X = x + 1, Z = z + 1, RelativeID = 8 });
            }

            result.ForEach(field => field.ID = field.X + (field.Z * _numberOfSquadsPerSide));

            return result;
        }
    }
}
