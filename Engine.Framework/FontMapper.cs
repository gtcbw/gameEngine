using Engine.Contracts;
using Graphics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Framework
{
    public sealed class FontMapper : IFontMapper
    {
        private readonly ITextureLoader _textureLoader;
        private readonly string _characterFolder;
        private Dictionary<char, ITexture> _characters = new Dictionary<char, ITexture>();

        public FontMapper(ITextureLoader textureLoader, 
            string characterFolder)
        {
            _textureLoader = textureLoader;
            _characterFolder = characterFolder;
            InitFont();
        }

        private void InitFont()
        {
            for(char a = 'A'; a<= 'Z'; a++)
            {
                _characters.Add(a, _textureLoader.LoadTexture($"{_characterFolder}\\{a}.png"));
            }

            for (char number = '0'; number <= '9'; number++)
            {
                _characters.Add(number, _textureLoader.LoadTexture($"{_characterFolder}\\{number}.png"));
            }
        }

        IEnumerable<int> IFontMapper.ConvertTextToGameFont(string text)
        {
            List<int> characters = new List<int>();

            foreach(char c in text)
            {
                if (_characters.Keys.Contains(c))
                    characters.Add(_characters[c].TextureId);
            }

            return characters;
        }
    }
}
