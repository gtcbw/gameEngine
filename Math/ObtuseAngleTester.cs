using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math
{
    public sealed class ObtuseAngleTester
    {
        public bool AngleIsObtuse(double[] vector1, double[] vector2)
        {
            return (vector1[0] * vector2[0]) + (vector1[1] * vector2[1]) + (vector1[2] * vector2[2]) <= 0;
        }
    }
}
