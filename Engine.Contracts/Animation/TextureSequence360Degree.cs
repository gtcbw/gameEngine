using System.Collections.Generic;

namespace Engine.Contracts.Animation
{
    public sealed class TextureSequence360Degree
    {
        public IDictionary<RotationDegrees, TextureSequence> TextureSequences { set; get; }
    }
}
