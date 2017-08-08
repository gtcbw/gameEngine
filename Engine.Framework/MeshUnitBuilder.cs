using Graphics.Contracts;
using Math.Contracts;

namespace Engine.Framework
{
    public sealed class MeshUnitBuilder
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private IHeightCalculator _heightCalculator;

        public MeshUnitBuilder(IBufferObjectFactory bufferObjectFactory,
            IHeightCalculator heightCalculator)
        {
            _bufferObjectFactory = bufferObjectFactory;
            _heightCalculator = heightCalculator;
        }

        public VertexBufferUnit CreateRelativeHeightMapUnit(int sideLengthInMeters, int metersPerTriangleSide, int startx, int startz)
        {
            int numberOfRows = sideLengthInMeters / metersPerTriangleSide;

            float [] vertices = new float[(numberOfRows + 1) * (numberOfRows + 1) * 3];

            ushort[] indices = new ushort[numberOfRows * numberOfRows * 6];

            for (int z = 0; z < numberOfRows + 1; z++)
            {
                for (int x = 0; x < numberOfRows + 1; x++)
                {
                    float xcoord = (x * metersPerTriangleSide) + startx;
                    float zcoord = (z * metersPerTriangleSide) + startz;

                    vertices[(((z * (numberOfRows + 1)) + x) * 3)] = xcoord;
                    vertices[(((z * (numberOfRows + 1)) + x) * 3) + 1] = (float)_heightCalculator.CalculateHeight(xcoord, zcoord);
                    vertices[(((z * (numberOfRows + 1)) + x) * 3) + 2] = zcoord;
                }
            }

            for(int rowz = 0; rowz < numberOfRows; rowz++)
            {
                for (int rowx = 0; rowx < numberOfRows; rowx++)
                {
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 0] = (ushort)(0 + (rowx + (rowz * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 1] = (ushort)(1 + (rowx + (rowz * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 2] = (ushort)(1 + (rowx + ((rowz + 1) * (numberOfRows + 1))));

                    indices[((rowx + (rowz * numberOfRows)) * 6) + 3] = (ushort)(0 + (rowx + (rowz * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 4] = (ushort)(1 + (rowx + ((rowz + 1) * (numberOfRows + 1))));
                    indices[((rowx + (rowz * numberOfRows)) * 6) + 5] = (ushort)(0 + (rowx + ((rowz + 1) * (numberOfRows + 1))));
                }
            }

            return new VertexBufferUnit
            {
                IndexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices),
                NumberOfTriangleCorners = indices.Length,
                VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices)
            };
        }
    }
}
