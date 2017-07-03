using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IRenderedSideSwitcher
    {
        void SwitchToFrontSide();

        void SwitchToBackSide();
    }
}
