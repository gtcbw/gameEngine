using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public sealed class WalkMotion
    {
        public Position Position { set; get; }

        public double MainDegreeXZ { set; get; }

        public double DriveDegreeY { set; get; }
    }
}
