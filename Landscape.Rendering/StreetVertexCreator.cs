using Engine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class StreetVertexCreator : IVertexByFieldCreator
    {
        float[] IVertexByFieldCreator.CreateVertices(FieldCoordinates field)
        {
            throw new NotImplementedException();
        }
    }
}
