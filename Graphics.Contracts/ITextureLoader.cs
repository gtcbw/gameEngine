using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface ITextureLoader
    {
        ITexture LoadTexture(string texturePath, bool mipMap);

        void DeleteTexture(ITexture texture);
    }
}
