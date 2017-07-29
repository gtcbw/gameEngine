using Engine.Contracts;
using Math.Contracts;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class FloorVertexCreator : IVertexByFieldCreator
    {
        private IHeightCalculator _heightCalculator;
        private int _numberOfRows;
        private int _metersPerTriangleSide;

        public FloorVertexCreator(IHeightCalculator heightCalculator,
        int numberOfRows,
        int metersPerTriangleSide)
        {
            _heightCalculator = heightCalculator;
            _numberOfRows = numberOfRows;
            _metersPerTriangleSide = metersPerTriangleSide;
        }

        float[] IVertexByFieldCreator.CreateVertices(FieldCoordinates field)
        {
            int startx = field.X * _metersPerTriangleSide * _numberOfRows;
            int startz = field.Z * _metersPerTriangleSide * _numberOfRows;

            float[] vertices = new float[(_numberOfRows + 1) * (_numberOfRows + 1) * 3];

            for (int z = 0; z < _numberOfRows + 1; z++)
            {
                for (int x = 0; x < _numberOfRows + 1; x++)
                {
                    float xcoord = (x * _metersPerTriangleSide) + startx;
                    float zcoord = (z * _metersPerTriangleSide) + startz;

                    vertices[(((z * (_numberOfRows + 1)) + x) * 3)] = xcoord;
                    vertices[(((z * (_numberOfRows + 1)) + x) * 3) + 1] = (float)_heightCalculator.CalculateHeight(xcoord, zcoord);
                    vertices[(((z * (_numberOfRows + 1)) + x) * 3) + 2] = zcoord;
                }
            }

            return vertices;
        }
    }
}
