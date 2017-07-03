using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Render.Contracts;

namespace Render
{
    public class Light : ILight
    {
        private EnableCap _lightIdentifier;
        private LightName _lightName;
        private double Red { set; get; }
        private double Green { set; get; }
        private double Blue { set; get; }
        private float _x;
        private float _y;
        private float _z;

        public Light(EnableCap lightIdentifier, LightName lightName, float red, float green, float blue, float x = 0.5f, float y = 1.0f, float z = 0.5f, bool diffuse = false)
        {
            _lightIdentifier = lightIdentifier;
            _lightName = lightName;
            GL.Light(_lightName, LightParameter.Ambient, new[] { red, green, blue });
            if (diffuse)
                GL.Light(_lightName, LightParameter.Diffuse, new[] { red, green, blue });
            _x = x;
            _y = y;
            _z = z;
        }

        void ILight.Enable()
        {
            GL.Enable(_lightIdentifier);      
        }

        void ILight.Disable()
        {
            GL.Disable(_lightIdentifier);
        }

        void ILight.SetPosition()
        {
            GL.Light(_lightName, LightParameter.Position, new[] { _x, _y, _z });
        }
    }
}
