using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts
{
    public interface IMeshUnitByFieldLoader
    {
        void UpdateMesh(IEnumerable<FieldCoordinates> addedFields, IEnumerable<FieldCoordinates> removedFields);
    }
}
