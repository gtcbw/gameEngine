using Engine.Contracts.Models;
using System.Collections.Generic;

namespace Engine.Framework
{
    public sealed class ModelCache : IModelRepository
    {
        private IModelRepository _modelRepository;
        private List<LoadedModel> _loadedModels;

        private sealed class LoadedModel
        {
            public Model Model { set; get; }
            public int Counter { set; get; }
        }

        public ModelCache(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        Model IModelRepository.Load(ModelInstanceDescription modelInstance)
        {
            LoadedModel loadedModel = _loadedModels.Find(x => x.Model.FileName.Equals(modelInstance.Filename));

            if (loadedModel == null)
            {
                Model model = _modelRepository.Load(modelInstance);
                loadedModel = new LoadedModel { Model = model, Counter = 1 };
                _loadedModels.Add(loadedModel);
            }
            else
                loadedModel.Counter++;

            return loadedModel.Model;
        }

        void IModelRepository.Delete(Model model)
        {
            LoadedModel loadedModel = _loadedModels.Find(x => x.Model.FileName == model.FileName);
            loadedModel.Counter--;

            if (loadedModel.Counter < 1)
                _modelRepository.Delete(model);
        }
    }
}
