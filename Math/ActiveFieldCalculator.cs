using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Math
{
    public sealed class ActiveFieldCalculator : IActiveFieldCalculator
    {
        private readonly int _lengthPerField;
        private readonly int _numberOfFieldsPerAreaSide;

        public ActiveFieldCalculator(int lengthPerField, int numberOfFieldsPerAreaSide)
        {
            _lengthPerField = lengthPerField;
            _numberOfFieldsPerAreaSide = numberOfFieldsPerAreaSide;
        }

        public IEnumerable<FieldCoordinates> CalculateActiveFields(Position position)
        {
            int x = (int)position.X / _lengthPerField;
            int z = (int)position.Z / _lengthPerField;

            List<FieldCoordinates> result = new List<FieldCoordinates>();

            if (x < 0 || z < 0 || z >= _numberOfFieldsPerAreaSide || x >= _numberOfFieldsPerAreaSide)
                return result;

            result.Add(new FieldCoordinates { X = x, Z = z, RelativeID = 4 });

            if (x > 0)
                result.Add(new FieldCoordinates { X = x - 1, Z = z, RelativeID = 3 });
            if (x < _numberOfFieldsPerAreaSide - 1)
                result.Add(new FieldCoordinates { X = x + 1, Z = z, RelativeID = 5 });

            if (z > 0)
            {
                result.Add(new FieldCoordinates { X = x, Z = z - 1, RelativeID = 1 });
                if (x > 0)
                    result.Add(new FieldCoordinates { X = x - 1, Z = z - 1, RelativeID = 0 });
                if (x < _numberOfFieldsPerAreaSide - 1)
                    result.Add(new FieldCoordinates { X = x + 1, Z = z - 1, RelativeID = 2 });
            }

            if (z < _numberOfFieldsPerAreaSide - 1)
            {
                result.Add(new FieldCoordinates { X = x, Z = z + 1, RelativeID = 7 });
                if (x > 0)
                    result.Add(new FieldCoordinates { X = x - 1, Z = z + 1, RelativeID = 6 });
                if (x < _numberOfFieldsPerAreaSide - 1)
                    result.Add(new FieldCoordinates { X = x + 1, Z = z + 1, RelativeID = 8 });
            }

            result.ForEach(field => field.ID = field.X + (field.Z * _numberOfFieldsPerAreaSide));

            return result;
        }
    }
}
