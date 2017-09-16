using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Engine.Contracts.PlayerMotion
{
    public class ClimbMotion
    {
        public Position Position { set; get; }

        public bool Done { set; get; }
    }

    public interface IVehicleClimber
    {
        void InitClimbUp(Position positionPlayer, Position positionVehicle);

        ClimbMotion GetClimbUpPosition();
    }
}
