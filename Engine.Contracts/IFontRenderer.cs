using System.Collections.Generic;

namespace Engine.Contracts
{
    public interface IFontRenderer
    {
        void RenderFont(IEnumerable<int> characterTextures, double startX, double startY);
    }
}
