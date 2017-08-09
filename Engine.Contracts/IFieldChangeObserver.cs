using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts
{
    public interface IFieldChangeObserver
    {
        void NotifyChangedFields(IEnumerable<FieldCoordinates> addedFields, IEnumerable<FieldCoordinates> removedFields);
    }
}
