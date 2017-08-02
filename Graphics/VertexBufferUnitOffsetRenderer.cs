using Graphics.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class VertexBufferUnitOffsetRenderer : IVertexBufferUnitRenderer
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

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, unit.IndexBufferId);

            GL.DrawElements(PrimitiveType.Triangles, unit.NumberOfIndices / 8, DrawElementsType.UnsignedShort, unit.NumberOfIndices / 4);

            GL.DisableClientState(ArrayCap.VertexArray);

            if (unit.TextureBufferId.HasValue)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
        }
    }
}
