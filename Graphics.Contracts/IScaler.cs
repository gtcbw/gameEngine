using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IScaler
    {
        void Scale(double x, double y, double z);
    }
}
