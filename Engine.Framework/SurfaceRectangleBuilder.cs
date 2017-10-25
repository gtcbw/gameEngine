using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public class SurfaceRectangleBuilder
    {
        public IEnumerable<Polygon> CreateRectangle(double leftCornerX, double leftCornerY, float lengthX, float lengthY, float textureZero = 0.001f, float textureOne = 0.999f, float z = -0.865f)
        {
            float startX = (float)(leftCornerX) - 0.5f;
            float startY = (float)(leftCornerY) - 0.5f;

            List<Polygon> triangles = new List<Polygon>();

            Polygon polygonOne = new Polygon { Normal = new Normal { Y = 1 } };
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex { Position = new VertexPosition { X = startX, Y = startY, Z = z }, TextureCoordinate = new VertexTextureCoordinate { X = textureZero, Y = textureOne } });
            vertices.Add(new Vertex { Position = new VertexPosition { X = startX + lengthX, Y = startY + lengthY, Z = z }, TextureCoordinate = new VertexTextureCoordinate { X = textureOne, Y = textureZero } });
            vertices.Add(new Vertex { Position = new VertexPosition { X = startX + lengthX, Y = startY, Z = z }, TextureCoordinate = new VertexTextureCoordinate { X = textureOne, Y = textureOne } });
            polygonOne.Vertices = vertices;

            triangles.Add(polygonOne);

            Polygon polygonTwo = new Polygon { Normal = new Normal { Y = 1 } };
            vertices = new List<Vertex>();
            vertices.Add(new Vertex { Position = new VertexPosition { X = startX, Y = startY, Z = z }, TextureCoordinate = new VertexTextureCoordinate { X = textureZero, Y = textureOne } });
            vertices.Add(new Vertex { Position = new VertexPosition { X = startX, Y = startY + lengthY, Z = z }, TextureCoordinate = new VertexTextureCoordinate { X = textureZero, Y = textureZero } });
            vertices.Add(new Vertex { Position = new VertexPosition { X = startX + lengthX, Y = startY + lengthY, Z = z }, TextureCoordinate = new VertexTextureCoordinate { X = textureOne, Y = textureZero } });
            polygonTwo.Vertices = vertices;

            triangles.Add(polygonTwo);

            return triangles;
        }
    }
}
