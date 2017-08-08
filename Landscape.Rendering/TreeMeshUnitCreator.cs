using Engine.Contracts;
using Graphics.Contracts;

namespace Landscape.Rendering
{
    public sealed class TreeMeshUnitCreator : IMeshUnitCreator
    {
        private IBufferObjectFactory _bufferObjectFactory;

        public TreeMeshUnitCreator(IBufferObjectFactory bufferObjectFactory)
        {
            _bufferObjectFactory = bufferObjectFactory;
        }

        VertexBufferUnit IMeshUnitCreator.CreateMeshUnit(float[] vertices)
        {
            int numberOfVertices = vertices.Length / 3;
            ushort[] indices = CreateIndexArray(numberOfVertices);
            float[] texcoords = CreateTextureCoordinates(numberOfVertices);

            return new VertexBufferUnit
            {
                IndexBufferId = _bufferObjectFactory.GenerateIndexBuffer(indices),
                NumberOfTriangleCorners = indices.Length,
                TextureBufferId = _bufferObjectFactory.GenerateTextureCoordBuffer(texcoords),
                VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices)
            };
        }

        void IMeshUnitCreator.DeleteMeshUnit(VertexBufferUnit unit)
        {
            _bufferObjectFactory.Delete(unit.VertexBufferId);
            _bufferObjectFactory.Delete(unit.IndexBufferId.Value);
            _bufferObjectFactory.Delete(unit.TextureBufferId.Value);
        }

        private float[] CreateTextureCoordinates(int numberOfVertices)
        {
            float[] texCoords = new float[numberOfVertices * 2];

            for (int i = 0; i < numberOfVertices / 4; i++)
            {
                texCoords[i * 8] = 1;
                texCoords[(i * 8) + 1] = 1;
                texCoords[(i * 8) + 2] = 1;
                texCoords[(i * 8) + 3] = 0;

                texCoords[(i * 8) + 4] = 0;
                texCoords[(i * 8) + 5] = 0;
                texCoords[(i * 8) + 6] = 0;
                texCoords[(i * 8) + 7] = 1;
            }

            return texCoords;
        }

        private ushort[] CreateIndexArray(int numberOfVertices)
        {
            ushort[] indices = new ushort[numberOfVertices / 4 * 6];

            for (int i = 0; i < numberOfVertices / 4; i++)
            {
                indices[i * 6] = (ushort)(i * 4);
                indices[(i * 6) + 1] = (ushort)((i * 4) + 1);
                indices[(i * 6) + 2] = (ushort)((i * 4) + 2);

                indices[(i * 6) + 3] = (ushort)((i * 4) + 2);
                indices[(i * 6) + 4] = (ushort)((i * 4) + 3);
                indices[(i * 6) + 5] = (ushort)(i * 4);
            }

            return indices;
        }
    }
}
