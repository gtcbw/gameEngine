using System.Collections.Generic;

namespace Engine.Contracts.Models
{
    public interface IFieldModelLoader
    {
        IEnumerable<ModelLocation> LoadModelsForField();
    }
}
