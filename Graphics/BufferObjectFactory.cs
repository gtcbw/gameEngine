using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class BufferObjectFactory : IBufferObjectFactory
    {
        uint IBufferObjectFactory.GenerateIndexBuffer(ushort[] indexArray)
        {
            uint indexBufferId;

            GL.GenBuffers(1, out indexBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexArray.Length * sizeof(ushort), indexArray, BufferUsageHint.StaticDraw);

            return indexBufferId;
        }

        uint IBufferObjectFactory.GenerateVertexBuffer(float[] vertexArray)
        {
            uint bufferId;

            GL.GenBuffers(1, out bufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexArray.Length * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);

            return bufferId;
        }

        uint IBufferObjectFactory.GenerateTextureCoordBuffer(float[] texCoordArray)
        {
            uint texCoordId;

            GL.GenBuffers(1, out texCoordId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordId);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoordArray.Length * sizeof(float), texCoordArray, BufferUsageHint.StaticDraw);

            return texCoordId;
        }

        uint IBufferObjectFactory.GenerateNormalBuffer(float[] normalArray)
        {
            uint normalId;

            GL.GenBuffers(1, out normalId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalId);
            GL.BufferData(BufferTarget.ArrayBuffer, normalArray.Length * sizeof(float), normalArray, BufferUsageHint.StaticDraw);

            return normalId;
        }

        void IBufferObjectFactory.Delete(uint bufferId)
        {
            GL.DeleteBuffers(1, ref bufferId);
        }
    }
}
