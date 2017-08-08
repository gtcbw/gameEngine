using Graphics.Contracts;
using System;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class VertexBufferUnitRenderer : IVertexBufferUnitRenderer
    {
        void IVertexBufferUnitRenderer.RenderMesh(VertexBufferUnit unit)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, unit.VertexBufferId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            if (unit.TextureBufferId.HasValue)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, unit.TextureBufferId.Value);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, IntPtr.Zero);
            }

            if (unit.NormalBufferId.HasValue)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, unit.NormalBufferId.Value);
                GL.EnableClientState(ArrayCap.NormalArray);
                GL.NormalPointer(NormalPointerType.Float, 0, IntPtr.Zero);
            }

            if (unit.IndexBufferId.HasValue)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, unit.IndexBufferId.Value);

                GL.DrawElements(PrimitiveType.Triangles, unit.NumberOfTriangleCorners, DrawElementsType.UnsignedShort, IntPtr.Zero);
            }
            else
                GL.DrawArrays(PrimitiveType.Triangles, 0, unit.NumberOfTriangleCorners);

            GL.DisableClientState(ArrayCap.VertexArray);

            if (unit.TextureBufferId.HasValue)
                GL.DisableClientState(ArrayCap.TextureCoordArray);

            if (unit.NormalBufferId.HasValue)
                GL.DisableClientState(ArrayCap.NormalArray);
        }
    }
}
