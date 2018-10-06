using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public sealed class RectangleUnitConverter : IRectangleUnitConverter
    {
        private IBufferObjectFactory _bufferObjectFactory;

        public RectangleUnitConverter(IBufferObjectFactory bufferObjectFactory)
        {
            _bufferObjectFactory = bufferObjectFactory;
        }

        public RectangleBufferUnit Convert(IEnumerable<Polygon> polygons)
        {
            RectangleBufferUnit unit = new RectangleBufferUnit();
            float[] vertices = new float[2 * 9];
            float[] texcoords = new float[2 * 6];

            int vertexIndex = 0, texcoordIndex = 0;

            foreach (Polygon polygon in polygons)
            {
                foreach (Vertex vertex in polygon.Vertices)
                {
                    vertices[vertexIndex++] = vertex.Position.X;
                    vertices[vertexIndex++] = vertex.Position.Y;
                    vertices[vertexIndex++] = vertex.Position.Z;

                    texcoords[texcoordIndex++] = vertex.TextureCoordinate.X;
                    texcoords[texcoordIndex++] = vertex.TextureCoordinate.Y;
                }
            }

            unit.VertexBufferId = _bufferObjectFactory.GenerateVertexBuffer(vertices);
            unit.TextureBufferId = _bufferObjectFactory.GenerateTextureCoordBuffer(texcoords);

            return unit;
        }

        public void Delete(RectangleBufferUnit unit)
        {
            _bufferObjectFactory.Delete(unit.VertexBufferId);
            _bufferObjectFactory.Delete(unit.TextureBufferId);
        }
    }
}
