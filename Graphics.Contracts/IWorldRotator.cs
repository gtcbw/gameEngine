using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IWorldRotator
    {
        void RotateX(double degree);

        void RotateY(double degree);

        void RotateZ(double degree);
    }
}
