using System.Collections.Generic;

namespace Engine.Contracts
{
    public interface IFontMapper
    {
        IEnumerable<int> ConvertTextToGameFont(string text);
    }
}
