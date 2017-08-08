using Graphics.Contracts;
using System;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class VertexBufferUnitOffsetRenderer : IVertexBufferUnitRenderer
    {
        private IIndexFactorProvider _factorProvider;
        private int _indexCountDivisor;

        public VertexBufferUnitOffsetRenderer(int indexCountDivisor, 
            IIndexFactorProvider factorProvider)
        {
            _factorProvider = factorProvider;
            _indexCountDivisor = indexCountDivisor;
        }

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

            if (unit.IndexBufferId.HasValue)
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, unit.IndexBufferId.Value);

            int numberOfIndices = unit.NumberOfTriangleCorners / _indexCountDivisor;
            int offset = unit.NumberOfTriangleCorners / _indexCountDivisor * _factorProvider.GetFactor() * 2;

            GL.DrawElements(PrimitiveType.Triangles,
                numberOfIndices, 
                DrawElementsType.UnsignedShort,
                (IntPtr)offset);

            GL.DisableClientState(ArrayCap.VertexArray);

            if (unit.TextureBufferId.HasValue)
                GL.DisableClientState(ArrayCap.TextureCoordArray);
        }
    }
}
