using Graphics.Contracts;
using System.Collections.Generic;

namespace Engine.Framework
{
    public sealed class TextureCache : ITextureLoader
    {
        private sealed class LoadedTexture
        {
            public ITexture Texture { set; get; }

            public string FileName { set; get; }
        }

        private ITextureLoader _textureLoader;

        private List<LoadedTexture> _loadedTextures;

        public TextureCache(ITextureLoader textureLoader)
        {
            _textureLoader = textureLoader;
            _loadedTextures = new List<LoadedTexture>();
        }

        ITexture ITextureLoader.LoadTexture(string texturePath)
        {
            LoadedTexture loadedTexture = _loadedTextures.Find(x => x.FileName.Equals(texturePath));

            if (loadedTexture == null)
            {
                ITexture texture = _textureLoader.LoadTexture(texturePath);
                loadedTexture = new LoadedTexture { Texture = texture, FileName = texturePath };
                _loadedTextures.Add(loadedTexture);
            }

            return loadedTexture.Texture;
        }

        void ITextureLoader.DeleteTexture(ITexture texture)
        {
            _textureLoader.DeleteTexture(texture);
        }
    }
}
