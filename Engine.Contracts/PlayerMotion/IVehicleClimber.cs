using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public class ClimbMotion
    {
        public double DegreeXZ { set; get; }

        public double DegreeY {set;get;} 

        public Position Position { set; get; }

        public bool Done { set; get; }
    }

    public interface IVehicleClimber
    {
        void InitClimbUp(Position positionPlayer, double degreeXZPlayer, double degreeYPlayer, Position positionVehicle, double degreeXZVehicle, double degreeYVehicle);

        ClimbMotion GetClimbUpPosition();
    }
}
