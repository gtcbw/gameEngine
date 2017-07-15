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
        private uint indexBufferId;

        public void CreateBuffer()
        {
            GL.GenBuffers(1, out bufferId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);

            float[] array = new float[4 * 3];

            array[0] = -5;
            array[1] = 0;
            array[2] = -5;

            array[3] = 5;
            array[4] = 0;
            array[5] = -5;

            array[6] = 5;
            array[7] = 0;
            array[8] = 5;

            array[9] = -5;
            array[10] = 0;
            array[11] = 5;

            GL.BufferData(BufferTarget.ArrayBuffer, 4 * 3 * sizeof(float), array, BufferUsageHint.StaticDraw);

            //////
            GL.GenBuffers(1, out indexBufferId);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);

            ushort[] indices = new ushort[6];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            GL.BufferData(BufferTarget.ElementArrayBuffer, 6 * sizeof(ushort), indices, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color3(0.5f, 0.8f, 0.2f);
           // // Bind vertex buffer:
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.VertexPointer(3, VertexPointerType.Float, 0, IntPtr.Zero);

          GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);

          GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedShort, IntPtr.Zero);

            GL.DisableClientState(ArrayCap.VertexArray);

            GL.Enable(EnableCap.Texture2D);
            GL.Color3(1.0f, 1.0f, 1.0f);
        }

        public void Delete()
        {
            GL.DeleteBuffers(1, ref bufferId);
        }
    }
}
