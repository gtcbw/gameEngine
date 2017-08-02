namespace Graphics.Contracts
{
    public class VertexBufferUnit
    {
        public uint VertexBufferId { set; get; }

        public uint IndexBufferId { set; get; }

        public uint? TextureBufferId { get; set; }

        public int NumberOfIndices { set; get; }
    }
}
