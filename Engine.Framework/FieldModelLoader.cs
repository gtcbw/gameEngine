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

        IEnumerable<ModelInstanceDescription> IFieldModelLoader.LoadModelsForField(int rowZ, int rowX)
        {
            return new List<ModelInstanceDescription>
            {
                new ModelInstanceDescription
                {
                    Filename = "box.mod",
                    RotationXZ = 0,
                    Position = new World.Model.Position
                    {
                        X = 50 + (rowX * 100),
                        Y = _heightCalculator.CalculateHeight(50 + (rowX * 100), 50 +  + (rowZ * 100)),
                        Z = 50 +  + (rowZ * 100)
                    }
                }
            };
        }
    }
}
