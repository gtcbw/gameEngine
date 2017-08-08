namespace Graphics.Contracts
{
    public class VertexBufferUnit
    {
        public uint VertexBufferId { set; get; }

        public uint? IndexBufferId { set; get; }

        public uint? TextureBufferId { get; set; }

        public uint? NormalBufferId { get; set; }

        public int NumberOfTriangleCorners { set; get; }
    }
}
