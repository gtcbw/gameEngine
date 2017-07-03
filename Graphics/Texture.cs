using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkContracts;
using BaseTypes;
using Render.Contracts;

namespace Render
{
    public class Texture : ITexture
    {
        public int TextureId
        {
            set;
            get;
        }

        public bool HasAlphaChannel
        {
            get;
            set;
        }

        public int ResolutionX
        {
            get;
            set;
        }

        public int ResolutionY
        {
            get;
            set;
        }
    }
}
