using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IWorldTranslator
    {
        void TranslateWorld(double x, double y, double z);

        void Store();

        void Reset();
    }
}
