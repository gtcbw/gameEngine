using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IDepthTestActivator
    {
        void Activate();
        void Deactivate();
    }
}
