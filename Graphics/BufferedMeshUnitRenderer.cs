using Graphics.Contracts;
using System;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class BufferedMeshUnitRenderer : IBufferedMeshUnitRenderer
    {
        void IBufferedMeshUnitRenderer.RenderMesh(BufferedMeshUnit unit)
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

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, unit.IndexBufferId);

            GL.DrawElements(PrimitiveType.Triangles, unit.NumberOfIndices, DrawElementsType.UnsignedShort, IntPtr.Zero);

            GL.DisableClientState(ArrayCap.VertexArray);

            if (unit.TextureBufferId.HasValue)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
        }
    }
}
