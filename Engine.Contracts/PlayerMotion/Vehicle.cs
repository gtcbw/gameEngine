using World.Model;

namespace Engine.Contracts.PlayerMotion
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
                return CollisionModel.RotationXZ;
            }
        }

        public ComplexShapeInstance CollisionModel { set; get; }

        public void UpdatePosition(Position position, double degreeXZ)
        {
            CollisionModel.Position = position;
            CollisionModel.RotationXZ = degreeXZ;
        }
    }
}
