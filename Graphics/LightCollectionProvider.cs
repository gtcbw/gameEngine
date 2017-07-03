using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;

namespace Render
{
    public class LightCollectionProvider : ILightCollectionProvider
    {
        ILightCollection ILightCollectionProvider.GetCollection(int levelId)
        {
            switch (levelId)
            {
                case 2:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.8f, 0.8f, 0.9f)
                    });
                case 3:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.9f, 0.9f, 0.8f)
                    });
                case 4:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.9f, 0.7f, 0.5f)
                    });
                case 6:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.8f, 0.6f, 0.3f)
                    });
                case 7:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.5f, 0.3f, 0.4f)
                    });
                case 8:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.7f, 0.7f, 0.9f)
                    });
                case 9:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.2f, 0.3f, 0.7f, x:-0.5f, z:-0.1f)
                    });
                case 10:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.1f, 0.2f, 0.6f)
                    });
                case 5:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.5f, 0.5f, 0.45f)
                    });
                case 11:
                case 12:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.95f, 1f, 0.9f),
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light1, OpenTK.Graphics.OpenGL.LightName.Light1, 0.75f, 0.8f, 0.6f, 0.1f, -0.7f, -0.1f, true)
                    });
                case 14:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.3f, 0.3f, 0.3f)
                    });
                case 15:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 1f, 0.2f, 0.1f)
                    });
                case 16:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.2f, 0.2f, 0.4f, 0.3f, 0.3f, 0.3f, true)
                    });
                case 17:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0,0.9f, 0.9f, 0.9f)
                    });
                case 18:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.9f, 0.9f, 0.8f)
                    });
                case 19:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.55f, 0.4f, 0.3f)
                    });
                case 23:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.7f, 0.7f, 0.9f, 0.5f, 0.7f,0.2f, true)
                    });
                case 24:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.9f, 0.8f, 0.65f)
                    });
                case 25:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.55f, 0.4f, 0.35f)
                    });
                case 26:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.5f, 0.6f, 0.7f)
                    });
                case 27:
                    return new LightCollection(new List<ILight> 
                    { 
                        //new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 1f, 0.7f, 0.6f, 0.5f, 0.2f,0.2f, true),
                        //new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.7f, 0.5f, 0.3f, 0.5f, -0.2f,0.2f)
                        //new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.6f, 0.35f, 0.4f),
                        //new Light(OpenTK.Graphics.OpenGL.EnableCap.Light1, OpenTK.Graphics.OpenGL.LightName.Light1, 0.2f, 0.1f, 0.1f, 0.1f, -0.5f, -0.1f, true)
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.55f, 0.45f, 0.35f),
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light1, OpenTK.Graphics.OpenGL.LightName.Light1, 0.2f, 0.15f, 0.15f, 0.1f, -0.5f, -0.1f, true)
                    });
                case 28:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 0.9f, 0.5f, 0.35f)
                    });
                default:
                    return new LightCollection(new List<ILight> 
                    { 
                        new Light(OpenTK.Graphics.OpenGL.EnableCap.Light0, OpenTK.Graphics.OpenGL.LightName.Light0, 1f, 1f, 1f)
                    });
            }
        }
    }
}
