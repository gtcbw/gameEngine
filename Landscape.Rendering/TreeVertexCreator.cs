using Engine.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class TreeVertexCreator : IVertexByFieldCreator
    {
        private IPositionGenerator _positionGenerator;
        private float[][] _treePrototype;

        public TreeVertexCreator(float[][] treePrototype, 
            IPositionGenerator positionGenerator)
        {
            _positionGenerator = positionGenerator;
            _treePrototype = treePrototype;
        }

        float[] IVertexByFieldCreator.CreateVertices(FieldCoordinates field)
        {
            IEnumerable<Position> positions = _positionGenerator.GeneratePositions(field);

            if (!positions.Any())
                return null;

            return TranslatePrototypeByPositions(positions);
        }

        private float[] TranslatePrototypeByPositions(IEnumerable<Position> positions)
        {
            int positionCount = positions.Count();
            float[] vertices = new float[12 * 8 * positionCount];

            int nextVertexIndex = 0;

            foreach(Position position in positions)
            {
                for (int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        vertices[nextVertexIndex + (i * 12 * positionCount) + (j * 3)] = (float)(_treePrototype[i][(j * 3)] + position.X);
                        vertices[nextVertexIndex + (i * 12 * positionCount) + (j * 3) + 1] = (float)(_treePrototype[i][(j * 3) + 1] + position.Y);
                        vertices[nextVertexIndex + (i * 12 * positionCount) + (j * 3) + 2] = (float)(_treePrototype[i][(j * 3) + 2] + position.Z);
                    }
                }

                nextVertexIndex += 12;
            }

            return vertices;
        }
    }
}
