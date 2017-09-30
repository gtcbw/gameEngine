using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public interface IVehicle
    {
        IReadOnlyPosition Position { get; }

        double DegreeXZ { get; }

        void UpdatePosition(Position position, double degreeXZ);
    }
}
