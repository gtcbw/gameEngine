using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public class SurfaceRectangleBuilder
    {
        public IEnumerable<Polygon> CreateRectangle(double leftCornerX, 
            double leftCornerY, 
            float lengthX, 
            float lengthY, 
            float textureXZero = 0.001f, 
            float textureXOne = 0.999f,
            float textureYZero = 0.001f,
            float textureYOne = 0.999f,
            float z = -0.865f)
        {
            float startX = (float)(leftCornerX) - 0.5f;
            float startY = (float)(leftCornerY) - 0.5f;

            List<Polygon> triangles = new List<Polygon>();

            Polygon polygonOne = new Polygon { Normal = new Normal { Y = 1 } };
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex
            {
                Position = new VertexPosition { X = startX, Y = startY, Z = z },
                TextureCoordinate = new VertexTextureCoordinate { X = textureXZero, Y = textureYOne }
            });
            vertices.Add(new Vertex
            {
                Position = new VertexPosition { X = startX + lengthX, Y = startY + lengthY, Z = z },
                TextureCoordinate = new VertexTextureCoordinate { X = textureXOne, Y = textureYZero }
            });
            vertices.Add(new Vertex
            {
                Position = new VertexPosition { X = startX + lengthX, Y = startY, Z = z },
                TextureCoordinate = new VertexTextureCoordinate { X = textureXOne, Y = textureYOne }
            });
            polygonOne.Vertices = vertices;

            triangles.Add(polygonOne);

            Polygon polygonTwo = new Polygon { Normal = new Normal { Y = 1 } };
            vertices = new List<Vertex>();
            vertices.Add(new Vertex
            {
                Position = new VertexPosition { X = startX, Y = startY, Z = z },
                TextureCoordinate = new VertexTextureCoordinate { X = textureXZero, Y = textureYOne }
            });
            vertices.Add(new Vertex
            {
                Position = new VertexPosition { X = startX, Y = startY + lengthY, Z = z },
                TextureCoordinate = new VertexTextureCoordinate { X = textureXZero, Y = textureYZero }
            });
            vertices.Add(new Vertex
            {
                Position = new VertexPosition { X = startX + lengthX, Y = startY + lengthY, Z = z },
                TextureCoordinate = new VertexTextureCoordinate { X = textureXOne, Y = textureYZero }
            });
            polygonTwo.Vertices = vertices;

            triangles.Add(polygonTwo);

            return triangles;
        }
    }
}
