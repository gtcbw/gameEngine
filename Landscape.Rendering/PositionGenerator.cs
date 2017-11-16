using Engine.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Landscape.Rendering
{
    public class PositionGenerator : IPositionGenerator
    {
        private IPositionFilter _positionFilter;
        private IBoolProvider _boolProvider;
        private IHeightCalculator _heightCalculator;
        private int _lengthOfFieldSide;
        private double _minimumDistanceOfTree;
        //private int _numberOfTreesInaRow;

        public PositionGenerator(IPositionFilter positionFilter,
            IBoolProvider boolProvider,
            IHeightCalculator heightCalculator,
            int lengthOfFieldSide,
            double minimumDistanceOfTrees)
        {
            _positionFilter = positionFilter;
            _boolProvider = boolProvider;
            _heightCalculator = heightCalculator;
            _lengthOfFieldSide = lengthOfFieldSide;
            _minimumDistanceOfTree = minimumDistanceOfTrees;
            //_numberOfTreesInaRow = (int)(lengthOfFieldSide / _minimumDistanceOfTree);
        }

        IEnumerable<Position> IPositionGenerator.GeneratePositions(FieldCoordinates field)
        {
            List<Position> positions = new List<Position>();
            double startX = field.X * _lengthOfFieldSide;
            double positionZ = field.Z * _lengthOfFieldSide;
            double maxZ = (field.Z + 1) * _lengthOfFieldSide;
            System.Random rand = new System.Random();
            while (positionZ < maxZ)
            {
                double positionX = startX;
                while (positionX < startX + _lengthOfFieldSide)
                {
                    Position position = new Position
                    {
                        X = positionX + rand.NextDouble() * 4,
                        Z = positionZ + rand.NextDouble() * 4
                    };

                    if (_positionFilter.IsValid(position) && _boolProvider.GetNext())
                    {
                        position.Y = _heightCalculator.CalculateHeight(positionX, positionZ);
                        positions.Add(position);
                    }

                    positionX += _minimumDistanceOfTree;
                }

                positionZ += _minimumDistanceOfTree;
            }

            return positions;
        }
        //IEnumerable<Position> IPositionGenerator.GeneratePositions(FieldCoordinates field)
        //{
        //    List<Position> positions = new List<Position>();
        //    double startX = field.X * _lengthOfFieldSide;
        //    double positionZ = field.Z * _lengthOfFieldSide;

        //    for (int z = 0; z < _numberOfTreesInaRow; z++)
        //    {
        //        double positionX = startX;
        //        for (int x = 0; x < _numberOfTreesInaRow; x++)
        //        {
        //            Position position = new Position
        //            {
        //                X = positionX,
        //                Z = positionZ
        //            };

        //            if (_positionFilter.IsValid(position) && _boolProvider.GetNext())
        //            {
        //                position.Y = _heightCalculator.CalculateHeight(positionX, positionZ);
        //                positions.Add(position);
        //            }

        //            positionX += _minimumDistanceOfTree;
        //        }

        //        positionZ += _minimumDistanceOfTree;
        //    }

        //    return positions;
        //}
    }
}
