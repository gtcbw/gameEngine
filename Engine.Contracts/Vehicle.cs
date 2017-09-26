using World.Model;

namespace Engine.Contracts
{
    public sealed class Vehicle : IVehicle
    {
        public IReadOnlyPosition Position
        {
            get
            {
                return CollisionModel.Position;
            }
         }

        public double DegreeXZ
        {
            get
            {
                return 0.0;
            }
        }

        public ComplexShapeInstance CollisionModel { set; get; }

        public void UpdatePosition(Position position, double degreeXZ)
        {
            CollisionModel.Position = position;

        }
    }
}
