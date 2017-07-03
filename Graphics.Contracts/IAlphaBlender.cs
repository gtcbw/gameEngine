using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IAlphaBlender
    {
        void BeginBlending();

        void EndBlending();

        void SetOpacity(double percent);
    }
}
