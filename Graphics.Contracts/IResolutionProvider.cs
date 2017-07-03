using System.Collections.Generic;

namespace Graphics.Contracts
{
    public interface IResolutionProvider
    {
        List<Resolution> GetSupportedResolutions();

        Resolution GetMaxResolution();
    }
}
