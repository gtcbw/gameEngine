using Engine.Contracts;
using Graphics.Contracts;
using Math.Contracts;
using System.Collections.Generic;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class DelayedFloorLoader
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private IHeightCalculator _heightCalculator;
        private IFloorCollection _floorCollection;
        private uint _indexBufferId;
        private int _numberOfIndices;

        public DelayedFloorLoader(IBufferObjectFactory bufferObjectFactory,
            IHeightCalculator heightCalculator,
            IFloorCollection floorCollection,
            int sideLengthInMeters, 
            int metersPerTriangleSide, 
            int startx, 
            int startz)
        {
            _bufferObjectFactory = bufferObjectFactory;
            _heightCalculator = heightCalculator;
            _floorCollection = floorCollection;

            ushort[] indices = CreateStandardIndexMesh(sideLengthInMeters / metersPerTriangleSide);
            _numberOfIndices = indices.Length;
            _indexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices);
        }

        public void UpdateMesh(IEnumerable<FieldCoordinates> fields)
        {
            foreach(FieldCoordinates field in fields)
            {

            }
        }

        private BufferedMeshUnit CreateRelativeHeightMapUnit(int numberOfRows, int metersPerTriangleSide, int startx, int startz)
        {
            float[] vertices = CreateVertices(numberOfRows, metersPerTriangleSide, startx, startz);

            return new BufferedMeshUnit
            {
                IndexBufferId = _indexBufferId,
                NumberOfIndices = _numberOfIndices,
                VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices)
            };
        }

        private float[] CreateVertices(int numberOfRows, int metersPerTriangleSide, int startx, int startz)
        {
            float[] vertices = new float[(numberOfRows + 1) * (numberOfRows + 1) * 3];

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

            return vertices;
        }

        private ushort[] CreateStandardIndexMesh(int numberOfRows)
        {
            ushort[] indices = new ushort[numberOfRows * numberOfRows * 6];

            for (int rowz = 0; rowz < numberOfRows; rowz++)
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

            return indices;
        }
    }
}
