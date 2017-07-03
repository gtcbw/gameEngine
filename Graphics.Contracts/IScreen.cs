using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public class Resolution
    {
        public int X { set; get; }

        public int Y { set; get; }
    }

    public interface IScreen
    {
        double AspectRatio { get; }

        Resolution CurrentResolution { get; }

        void ChangeResolution(Resolution desiredResolution);
    }
}
