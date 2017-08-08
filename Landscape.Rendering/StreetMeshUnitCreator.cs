using Engine.Contracts;
using Graphics.Contracts;

namespace Landscape.Rendering
{
    public sealed class StreetMeshUnitCreator : IMeshUnitCreator
    {
        private IBufferObjectFactory _bufferObjectFactory;
        private uint _indexBufferId;
        private uint _texCoordBufferId;
        private int _numberOfIndices;

        public StreetMeshUnitCreator(IBufferObjectFactory bufferObjectFactory,
        int numberOfQuads)
        {
            _bufferObjectFactory = bufferObjectFactory;

            ushort[] indices = CreateIndexArray(numberOfQuads);
            _numberOfIndices = indices.Length;
            _indexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices);

            float[] texcoords = CreateTextureCoordinates(numberOfQuads);
            _texCoordBufferId = _bufferObjectFactory.GenerateTextureCoordBuffer(texcoords);
        }

        VertexBufferUnit IMeshUnitCreator.CreateMeshUnit(float[] vertices)
        {
            return new VertexBufferUnit
            {
                IndexBufferId = _indexBufferId,
                NumberOfTriangleCorners = _numberOfIndices,
                TextureBufferId = _texCoordBufferId,
                VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices)
            };
        }

        void IMeshUnitCreator.DeleteMeshUnit(VertexBufferUnit unit)
        {
            _bufferObjectFactory.Delete(unit.VertexBufferId);
        }

        private float[] CreateTextureCoordinates(int numberOfQuads)
        {
            float[] texCoords = new float[(numberOfQuads + 1) * 4];

            for (int i = 0; i <= numberOfQuads; i++)
            {
                if (i % 2 == 0)
                {
                    texCoords[i * 4] = 0;
                    texCoords[(i * 4) + 1] = 0;
                    texCoords[(i * 4) + 2] = 1;
                    texCoords[(i * 4) + 3] = 0;
                }
                else
                {
                    texCoords[i * 4] = 0;
                    texCoords[(i * 4) + 1] = 1;
                    texCoords[(i * 4) + 2] = 1;
                    texCoords[(i * 4) + 3] = 1;
                }
            }

            return texCoords;
        }

        private ushort[] CreateIndexArray(int numberOfQuads)
        {
            ushort[] indices = new ushort[numberOfQuads * 6];

            for (int i = 0; i < numberOfQuads; i++)
            {
                indices[i * 6] = (ushort)(i * 2);
                indices[(i * 6) + 1] = (ushort)(((i + 1) * 2) + 1);
                indices[(i * 6) + 2] = (ushort)((i + 1) * 2);

                indices[(i * 6) + 3] = (ushort)(i * 2);
                indices[(i * 6) + 4] = (ushort)((i * 2) + 1);
                indices[(i * 6) + 5] = (ushort)(((i + 1) * 2) + 1);
            }

            return indices;
        }
    }
}
