using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public sealed class WalkMotion
    {
        public Position Position { set; get; }

        public double DegreeXZ { set; get; }

        public double DegreeY { set; get; }

        public Vector2D VectorXZ { set; get; }
    }
}
