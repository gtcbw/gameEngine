using Engine.Contracts;
using Newtonsoft.Json;
using System.IO;

namespace Engine.Framework
{
    public sealed class ModelLoader : IModelLoader
    {
        private string _folder;

        public ModelLoader(string folder)
        {
            _folder = folder;
        }

        Model IModelLoader.LoadModel(string filename)
        {
            return JsonConvert.DeserializeObject<Model>(File.ReadAllText($"{_folder}\\{filename}"));
        }
    }
}
