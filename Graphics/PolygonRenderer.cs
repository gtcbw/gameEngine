using System.Collections.Generic;
using Graphics.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class PolygonRenderer : IPolygonRenderer
    {
        void IPolygonRenderer.RenderPolygons(List<Polygon> polygons)
        {
            foreach (Polygon polygon in polygons)
            {
                GL.Begin(PrimitiveType.Polygon);

                foreach (Vertex vertex in polygon.Vertices)
                {
                    GL.TexCoord2(vertex.TextureCoordinate.X, vertex.TextureCoordinate.Y);
                    GL.Vertex3(vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
                }

                GL.End();
            }
        }
    }
}
