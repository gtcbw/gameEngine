using World.Model;

namespace Engine.Contracts.Models
{
    public sealed class ModelInstanceDescription
    {
        public string Filename { set; get; }

        public Position Position { set; get; }

        public double RotationXZ { set; get; }
    }
}
