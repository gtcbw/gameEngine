using Math.Contracts;
using System;
using System.Collections.Generic;
using World.Model;

namespace Math
{
    public sealed class FieldCoordinates
    {
        public int X { set; get; }
        public int Z { set; get; }
    }
    public sealed class ActiveFieldCalculator : IActiveFieldCalculator
    {
        private readonly int _lengthPerSquad;
        private readonly int _numberOfSquadsPerSide;

        public ActiveFieldCalculator(int lengthPerSquad, int numberOfSquadsPerSide)
        {
            _lengthPerSquad = lengthPerSquad;
            _numberOfSquadsPerSide = numberOfSquadsPerSide;
        }

        public IEnumerable<int> CalculateActiveFields(Position position)
        {
            int x = (int)position.X / _lengthPerSquad;
            int z = (int)position.Z / _lengthPerSquad;

            int centerField = x + (z * _numberOfSquadsPerSide);

            List<int> result = new List<int>();

            result.Add(centerField);

            if (x > 0)
                result.Add(centerField - 1);
            if (x < _numberOfSquadsPerSide - 1)
                result.Add(centerField + 1);

            if (z > 0)
            {
                result.Add(centerField - _numberOfSquadsPerSide);
                if (x > 0)
                    result.Add(centerField - _numberOfSquadsPerSide - 1);
                if (x < _numberOfSquadsPerSide - 1)
                    result.Add(centerField - _numberOfSquadsPerSide + 1);
            }

            if (z < _numberOfSquadsPerSide - 1)
            {
                result.Add(centerField + _numberOfSquadsPerSide);
                if (x > 0)
                    result.Add(centerField + _numberOfSquadsPerSide - 1);
                if (x < _numberOfSquadsPerSide - 1)
                    result.Add(centerField + _numberOfSquadsPerSide + 1);
            }

            return result;
        }
    }
}
