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
        void InitClimb(IReadOnlyPosition positionPlayer, double degreeXZPlayer, double degreeYPlayer, IReadOnlyPosition positionVehicle, double degreeXZVehicle, double degreeYVehicle);

        ClimbMotion GetClimbPosition();
    }
}
