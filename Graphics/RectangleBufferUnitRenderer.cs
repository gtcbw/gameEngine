using Graphics.Contracts;
using System;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class RectangleBufferUnitRenderer : IRectangleBufferUnitRenderer
    {
        public void RenderUnit(RectangleBufferUnit unit)
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, unit.VertexBufferId);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ArrayBuffer, unit.TextureBufferId);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.TextureCoordArray);
        }
    }
}
