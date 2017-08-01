using Engine.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class TreeVertexCreator : IVertexByFieldCreator
    {
        private IPositionFilter _positionFilter;
        private IHeightCalculator _heightCalculator;
        private float[][] _treePrototype;
        private int _lengthOfFieldSide;
        private double _minimumDistanceOfTree;

        public TreeVertexCreator(IPositionFilter positionFilter,
            IHeightCalculator heightCalculator,
            float[][] treePrototype, 
            int lengthOfFieldSide,
            double minimumDistanceOfTrees)
        {
            _positionFilter = positionFilter;
            _heightCalculator = heightCalculator;
            _treePrototype = treePrototype;
            _lengthOfFieldSide = lengthOfFieldSide;
            _minimumDistanceOfTree = minimumDistanceOfTrees;
        }

        float[] IVertexByFieldCreator.CreateVertices(FieldCoordinates field)
        {
            int startX = field.X * _lengthOfFieldSide;
            int startZ = field.Z * _lengthOfFieldSide;

            List<Position> positions = new List<Position>();

            int numberOfTreesInaRow = (int)(_lengthOfFieldSide / _minimumDistanceOfTree);

            double positionZ = startZ;
            for (int z = 0; z < numberOfTreesInaRow; z++)
            {
                double positionX = startX;
                for (int x = 0; x < numberOfTreesInaRow; x++)
                {
                    Position position = new Position
                    {
                        X = positionX,
                        Z = positionZ
                    };

                    if (_positionFilter.IsValid(position))
                    {
                        position.Y = _heightCalculator.CalculateHeight(positionX, positionZ);
                        positions.Add(position);
                    }

                    positionX += _minimumDistanceOfTree;
                }

                positionZ += _minimumDistanceOfTree; 
            }

            return TranslatePrototypeByPositions(positions);
        }

        private float[] TranslatePrototypeByPositions(IEnumerable<Position> positions)
        {
            int positionCount = positions.Count();
            float[] vertices = new float[12 * 8 * positionCount];

            foreach(Position position in positions)
            {
                for (int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        vertices[(i * 12 * positionCount) + (j * 3)] = (float) (_treePrototype[i][(j * 3)] + position.X);
                        vertices[(i * 12 * positionCount) + (j * 3) + 1] = (float)(_treePrototype[i][(j * 3) + 1] + position.Y);
                        vertices[(i * 12 * positionCount) + (j * 3) + 2] = (float)(_treePrototype[i][(j * 3) + 2] + position.Z);
                    }
                }
            }

            return vertices;
        }
    }
}
