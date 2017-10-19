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

            public int Counter { set; get; }
        }

        private ITextureLoader _textureLoader;

        private List<LoadedTexture> _loadedTextures = new List<LoadedTexture>();

        public TextureCache(ITextureLoader textureLoader)
        {
            _textureLoader = textureLoader;
        }

        ITexture ITextureLoader.LoadTexture(string texturePath, bool mipmap, bool fullPath)
        {
            LoadedTexture loadedTexture = _loadedTextures.Find(x => x.FileName.Equals(texturePath));

            if (loadedTexture == null)
            {
                ITexture texture = _textureLoader.LoadTexture(texturePath, mipmap, fullPath);
                loadedTexture = new LoadedTexture { Texture = texture, FileName = texturePath, Counter = 1 };
                _loadedTextures.Add(loadedTexture);
            }
            else
                loadedTexture.Counter++;

            return loadedTexture.Texture;
        }

        void ITextureLoader.DeleteTexture(ITexture texture)
        {
            LoadedTexture loadedTexture = _loadedTextures.Find(x => x.Texture.TextureId == texture.TextureId);
            loadedTexture.Counter--;

            if (loadedTexture.Counter > 0)
                return;

            _textureLoader.DeleteTexture(texture);
            _loadedTextures.Remove(loadedTexture);
        }
    }
}
