using Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.StaticGameContent
{
    public static class VehicleListProvider
    {
        public static IEnumerable<Vehicle> GetVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle { Position = new World.Model.Position { X = 100, Y = 1, Z = 100 } }
            };
        }
    }
}
