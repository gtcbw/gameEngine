using OpenTK.Graphics.OpenGL;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class BufferedMeshUnitFactory : IBufferedMeshUnitFactory
    {
        BufferedMeshUnit IBufferedMeshUnitFactory.GenerateBuffer(float[] vertexArray, ushort[] indexArray)
        {
            uint bufferId;
            uint indexBufferId;

            GL.GenBuffers(1, out bufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, bufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexArray.Length * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out indexBufferId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexArray.Length * sizeof(ushort), indexArray, BufferUsageHint.StaticDraw);

            return new BufferedMeshUnit { IndexBufferId = indexBufferId, VertexBufferId = bufferId };
        }

        void IBufferedMeshUnitFactory.Delete(BufferedMeshUnit meshUnit)
        {
            uint id = meshUnit.IndexBufferId;
            GL.DeleteBuffers(1, ref id);
            id = meshUnit.VertexBufferId;
            GL.DeleteBuffers(1, ref id);
        }
    }
}
