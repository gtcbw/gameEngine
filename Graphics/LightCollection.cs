using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Render.Contracts;

namespace Render
{
    public class LightCollection : ILightCollection
    {
        private List<ILight> _lights;

        public LightCollection(List<ILight> lights)
        {
            _lights = lights;

            foreach (ILight light in _lights)
            {
                light.Enable();
            }
        }

        void ILightCollection.Enable()
        {
            GL.Enable(EnableCap.Lighting);
            foreach (ILight light in _lights)
            {
                light.SetPosition();
            }
        }

        void ILightCollection.Disable()
        {
            GL.Disable(EnableCap.Lighting);
        }

        private EnableCap DetermineIdentifier(int lightIndex)
        {
            switch (lightIndex)
            {
                case 0:
                    return EnableCap.Light0;
                case 1:
                    return EnableCap.Light1;
                case 2:
                    return EnableCap.Light2;
                case 3:
                    return EnableCap.Light3;
                case 4:
                    return EnableCap.Light4;
                case 5:
                    return EnableCap.Light5;
                case 6:
                    return EnableCap.Light6;
                case 7:
                    return EnableCap.Light7;
                default:
                    throw new NotSupportedException("Light identifier is not supported!");
            }
        }
    }
}
