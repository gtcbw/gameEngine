using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface ILight
    {
        void SetPosition();
        void Enable();
        void Disable();
    }
}
