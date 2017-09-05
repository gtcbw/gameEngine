using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public sealed class VehicleMotion
    {
        public Position Position { set; get; }

        public double Speed { set; get; }

        public double MainDegreeXZ { set; get; }

        public double SteeringWheelAngle { set; get; }
    }
}
