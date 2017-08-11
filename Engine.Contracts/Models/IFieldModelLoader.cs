using System.Collections.Generic;

namespace Engine.Contracts.Models
{
    public interface IFieldModelLoader
    {
        IEnumerable<ModelInstanceDescription> LoadModelsForField(int rowZ, int rowX);
    }
}
