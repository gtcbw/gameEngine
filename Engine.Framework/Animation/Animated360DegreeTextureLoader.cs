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
        string basePath = "textures\\";

        public Animated360DegreeTextureLoader(ITextureLoader textureLoader)
        {
            _textureLoader = textureLoader;
        }

        TextureSequence360Degree IAnimated360DegreeTextureLoader.LoadAnimatedTexture(string animationName)
        {
            
            Dictionary<RotationDegrees, TextureSequence> animations = new Dictionary<RotationDegrees, TextureSequence>();

            foreach(RotationDegrees rotationDegrees in Enum.GetValues(typeof(RotationDegrees)))
            {
                var subFolder = $"{basePath}{animationName}\\{rotationDegrees}";

                if (!Directory.Exists(subFolder))
                    continue;

                List<ITexture> textures = new List<ITexture>();
                foreach (string fileName in Directory.GetFiles(subFolder).OrderBy(x=>x))
                {
                    textures.Add(_textureLoader.LoadTexture(fileName, fullPath: true));
                }

                animations.Add(rotationDegrees, new TextureSequence { Textures = textures.ToArray() });
            }

            return new TextureSequence360Degree { TextureSequences = animations };
        }
    }
}
