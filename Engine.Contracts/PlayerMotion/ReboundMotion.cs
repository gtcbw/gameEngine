using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public sealed class ReboundMotion
    {
        public Position Position { set; get; }

        public double Speed { set; get; }

        public double MainViewDegreeXZ { set; get; }

        public double RelativeViewDegreeXZ { set; get; }

        public double MovementDegree { set; get; }

        public double ViewDegreeY { set; get; }
    }
}
