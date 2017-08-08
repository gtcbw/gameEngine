using Engine.Contracts;
using Graphics.Contracts;

namespace Landscape.Rendering
{
    public sealed class FloorMeshUnitCreator : IMeshUnitCreator
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private uint _indexBufferId;
        private int _numberOfIndices;

        private int _numberOfRows;

        public FloorMeshUnitCreator(IBufferObjectFactory bufferObjectFactory,
            int numberOfRows)
        {
            _bufferObjectFactory = bufferObjectFactory;
            _numberOfRows = numberOfRows;

            ushort[] indices = CreateStandardIndexMesh();
            _numberOfIndices = indices.Length;
            _indexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices);
        }
        VertexBufferUnit IMeshUnitCreator.CreateMeshUnit(float[] vertices)
        {
            return new VertexBufferUnit
            {
                IndexBufferId = _indexBufferId,
                NumberOfTriangleCorners = _numberOfIndices,
                VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices)
            };
        }

        void IMeshUnitCreator.DeleteMeshUnit(VertexBufferUnit unit)
        {
            _bufferObjectFactory.Delete(unit.VertexBufferId);
        }

        private ushort[] CreateStandardIndexMesh()
        {
            ushort[] indices = new ushort[_numberOfRows * _numberOfRows * 6];

            for (int rowz = 0; rowz < _numberOfRows; rowz++)
            {
                for (int rowx = 0; rowx < _numberOfRows; rowx++)
                {
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 0] = (ushort)(0 + (rowx + (rowz * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 1] = (ushort)(1 + (rowx + (rowz * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 2] = (ushort)(1 + (rowx + ((rowz + 1) * (_numberOfRows + 1))));

                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 3] = (ushort)(0 + (rowx + (rowz * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 4] = (ushort)(1 + (rowx + ((rowz + 1) * (_numberOfRows + 1))));
                    indices[((rowx + (rowz * _numberOfRows)) * 6) + 5] = (ushort)(0 + (rowx + ((rowz + 1) * (_numberOfRows + 1))));
                }
            }

            return indices;
        }
    }
}
