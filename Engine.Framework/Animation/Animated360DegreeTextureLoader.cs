using Engine.Contracts.Animation;
using Graphics.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.Framework.Animation
{
    public sealed class Animated360DegreeTextureLoader : IAnimated360DegreeTextureLoader
    {
        private readonly ITextureLoader _textureLoader;
        private readonly string _basePath;

        public Animated360DegreeTextureLoader(ITextureLoader textureLoader,
            string basePath)
        {
            _textureLoader = textureLoader;
            _basePath = basePath;
        }

        TextureSequence360Degree IAnimated360DegreeTextureLoader.LoadAnimatedTexture(string name)
        {
            string animationName = $"{_basePath}\\{name}";

            Dictionary<RotationDegrees, TextureSequence> animations = new Dictionary<RotationDegrees, TextureSequence>();

            foreach(RotationDegrees rotationDegrees in Enum.GetValues(typeof(RotationDegrees)))
            {
                var subFolder = $"{animationName}\\{rotationDegrees}";

                if (!Directory.Exists(subFolder))
                    continue;

                List<ITexture> textures = new List<ITexture>();
                foreach (string fileName in Directory.GetFiles(subFolder).OrderBy(x=>x))
                {
                    textures.Add(_textureLoader.LoadTexture(fileName));
                }

                animations.Add(rotationDegrees, new TextureSequence { Textures = textures.ToArray() });
            }

            return new TextureSequence360Degree { TextureSequences = animations };
        }
    }
}
