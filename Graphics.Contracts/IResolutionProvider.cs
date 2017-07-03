using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface IResolutionProvider
    {
        List<Resolution> GetSupportedResolutions();

        Resolution GetMaxResolution();
    }
}
