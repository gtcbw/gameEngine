using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Contracts
{
    public interface IFog
    {
        void StartFog();

        void StopFog();

        void SetColor(float[] color);
    }
}
