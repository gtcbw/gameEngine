using Engine.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Framework
{
    public sealed class ModelCache : IModelRepository
    {
        private Dictionary<string, LoadedModel> _loadedModels = new Dictionary<string, LoadedModel>();
        private readonly IModelLoader _modelLoader;

        private sealed class LoadedModel
        {
            public Model Model { set; get; }

            public int Counter { set; get; }
        }

        public ModelCache(IModelLoader modelLoader)
        {
            _modelLoader = modelLoader;
        }

        ModelInstance IModelRepository.Load(ModelInstanceDescription modelInstance)
        {
            LoadedModel loadedModel;

            if (!_loadedModels.Keys.Contains(modelInstance.Filename))
            {
                Model model = _modelLoader.Load(modelInstance.Filename);
                loadedModel = new LoadedModel { Model = model, Counter = 1 };
                _loadedModels.Add(modelInstance.Filename, loadedModel);
            }
            else
            {
                loadedModel = _loadedModels[modelInstance.Filename];
                loadedModel.Counter++;
            }

            return new ModelInstance
            {
                FileName = modelInstance.Filename,
                RenderUnits = loadedModel.Model.RenderUnits,
                CollisionModelInstance = new World.Model.ComplexShapeInstance
                {
                    Position = modelInstance.Position,
                    RotationXZ = modelInstance.RotationXZ,
                    ComplexShape = loadedModel.Model.CollisionModel
                }
            };
        }

        void IModelRepository.Delete(ModelInstance model)
        {
            LoadedModel loadedModel = _loadedModels[model.FileName];
            loadedModel.Counter--;

            if (loadedModel.Counter < 1)
            {
                _modelLoader.Delete(loadedModel.Model);
                _loadedModels.Remove(model.FileName);
            }
        }
    }
}
