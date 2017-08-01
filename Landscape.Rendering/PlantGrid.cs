using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landscape.Rendering
{
    public class PlantGrid
    {
        private bool[][] _values;
        private int _metersPerGridField;

        public PlantGrid(bool [][] values, int metersPerGridField)
        {
            _values = values;
            _metersPerGridField = metersPerGridField;
        }

        public bool AreaIsFree(double x, double z)
        {
            return false;
        }
    }
}
