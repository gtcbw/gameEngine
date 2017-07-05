using System.Collections.Generic;
using System.Linq;
using Graphics.Contracts;
using OpenTK;

namespace Graphics
{
    public sealed class ResolutionProvider : IResolutionProvider
    {
        public List<Resolution> GetSupportedResolutions()
        {
            List<Resolution> resolutions = new List<Resolution>();
            foreach (DisplayResolution resolution in DisplayDevice.Default.AvailableResolutions)
            {
                if (!resolutions.Any(x => x.X == resolution.Width && x.Y == resolution.Height))
                    resolutions.Add(new Resolution { X = resolution.Width, Y = resolution.Height });
            } 
            return resolutions;
        }

        public Resolution GetMaxResolution()
        {
            List<Resolution> resolutions = GetSupportedResolutions();

            int maxResolutionSum = resolutions.Max(x => x.X + x.Y);

            return resolutions.Find(x=>x.X + x.Y == maxResolutionSum);
        }
    }
}
