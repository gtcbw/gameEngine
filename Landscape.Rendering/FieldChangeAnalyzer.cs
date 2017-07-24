using Engine.Contracts;
using System.Collections.Generic;
using System.Linq;
using World.Model;

namespace Landscape.Rendering
{
    public sealed class FieldChangeAnalyzer : IFieldChangeAnalyzer
    {
        IEnumerable<FieldCoordinates> IFieldChangeAnalyzer.FindAddedFields(IEnumerable<FieldCoordinates> oldFields, IEnumerable<FieldCoordinates> newFields)
        {
            List<FieldCoordinates> addedFields = new List<FieldCoordinates>();

            foreach (FieldCoordinates field in newFields)
            {
                if (oldFields.FirstOrDefault(x => x.ID == field.ID) == null)
                    addedFields.Add(field);
            }

            return addedFields;
        }

        IEnumerable<FieldCoordinates> IFieldChangeAnalyzer.FindRemovedFields(IEnumerable<FieldCoordinates> oldFields, IEnumerable<FieldCoordinates> newFields)
        {
            List<FieldCoordinates> removedFields = new List<FieldCoordinates>();

            foreach (FieldCoordinates field in oldFields)
            {
                if (newFields.FirstOrDefault(x => x.ID == field.ID) == null)
                    removedFields.Add(field);
            }

            return removedFields;
        }
    }
}
