using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Graphics
{
    public sealed class VertexBufferFactory
    {
        private uint bufferId;

        public void CreateBuffer()
        {
            GL.GenBuffers(1, out bufferId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);

            float[] array = new float[1000 * 3];

            GL.BufferData(BufferTarget.ArrayBuffer, 1000 * 3 * sizeof(float), array, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            // Bind vertex buffer:
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 1000 * 3);

            GL.DisableClientState(ArrayCap.VertexArray);
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref bufferId);
        }
    }
}
