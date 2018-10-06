using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Contracts
{
    public interface IRectangleUnitConverter
    {
        RectangleBufferUnit Convert(IEnumerable<Polygon> polygons);

        void Delete(RectangleBufferUnit unit);
    }
}
