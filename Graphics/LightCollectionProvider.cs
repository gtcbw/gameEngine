using System.Collections.Generic;
using Graphics.Contracts;

namespace Graphics
{
    public sealed class LightCollectionProvider : ILightCollectionProvider
    {
        ILightCollection ILightCollectionProvider.GetCollection()
        {
                return new LightCollection(new List<ILight> 
                { 
                    new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 1f, 1f, 1f)
                });
        }
    }
}
