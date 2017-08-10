using Engine.Contracts.Models;
using Math.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Framework
{
    public sealed class FieldModelLoader : IFieldModelLoader
    {
        private IHeightCalculator _heightCalculator;

        public FieldModelLoader(IHeightCalculator heightCalculator)
        {
            _heightCalculator = heightCalculator;
        }

        IEnumerable<ModelLocation> IFieldModelLoader.LoadModelsForField()
        {
            return new List<ModelLocation>
            {
                new ModelLocation { Filename = "box.json", Position = new World.Model.Position
                {
                    X = 150,
                    Y = _heightCalculator.CalculateHeight(150, 150),
                    Z = 150 }
                }
            };
        }
    }
}
