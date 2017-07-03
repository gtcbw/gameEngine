using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface ITexture
    {
        int TextureId { get; }

        bool HasAlphaChannel { get; }

        int ResolutionX { get; }

        int ResolutionY { get; }
    }
}
