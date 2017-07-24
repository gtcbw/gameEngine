using System.Collections.Generic;
using World.Model;

namespace Engine.Contracts
{
    public interface IFieldChangeAnalyzer
    {
        IEnumerable<FieldCoordinates> FindAddedFields(IEnumerable<FieldCoordinates> oldFields, IEnumerable<FieldCoordinates> newFields);

        IEnumerable<FieldCoordinates> FindRemovedFields(IEnumerable<FieldCoordinates> oldFields, IEnumerable<FieldCoordinates> newFields);
    }
}
