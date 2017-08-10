using World.Model;

namespace Engine.Contracts.Models
{
    public sealed class ModelLocation
    {
        public string Filename { set; get; }

        public Position Position { set; get; }
    }
}
