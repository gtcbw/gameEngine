using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Render.Contracts
{
    public interface ITextureTranslator
    {
        void TranslateTexture(double x, double y);

        void Store();

        void Reset();
    }
}
