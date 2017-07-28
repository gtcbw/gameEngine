using Graphics.Contracts;
using System;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class BufferedMeshUnitRenderer : IBufferedMeshUnitRenderer
    {
        void IBufferedMeshUnitRenderer.RenderMesh(BufferedMeshUnit unit)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color3((float)(1.0 / 255.0 * 50.0), (float)(1.0 / 255.0 * 180.0), (float)(1.0 / 255.0 * 50.0));

            GL.BindBuffer(BufferTarget.ArrayBuffer, unit.VertexBufferId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, unit.IndexBufferId);

            GL.DrawElements(PrimitiveType.Triangles, unit.NumberOfIndices, DrawElementsType.UnsignedShort, IntPtr.Zero);

            GL.DisableClientState(ArrayCap.VertexArray);

            GL.Enable(EnableCap.Texture2D);
            GL.Color3(1.0f, 1.0f, 1.0f);
        }
    }
}
