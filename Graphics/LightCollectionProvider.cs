using System.Collections.Generic;
using Graphics.Contracts;

namespace Graphics
{
    public class LightCollectionProvider : ILightCollectionProvider
    {
        ILightCollection ILightCollectionProvider.GetCollection(int levelId)
        {
            switch (levelId)
            {
                default:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 1f, 1f, 1f)
                    });
            }
        }
    }
}
