using System.Collections.Generic;
using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class TriangleRenderer : IPolygonRenderer
    {
        void IPolygonRenderer.RenderPolygons(IEnumerable<Polygon> polygons)
        {
            GL.Begin(PrimitiveType.Triangles);

            foreach (Polygon polygon in polygons)
            {
                GL.Normal3(polygon.Normal.X, polygon.Normal.Y, polygon.Normal.Z);

                foreach (Vertex vertex in polygon.Vertices)
                {
                    GL.TexCoord2(vertex.TextureCoordinate.X, vertex.TextureCoordinate.Y);
                    GL.Vertex3(vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
                }
            }

            GL.End();
        }
    }
}
