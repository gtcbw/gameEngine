using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Render.Contracts;
using OpenTK.Graphics.OpenGL;

namespace Render
{
    public class WorldTranslator : IWorldTranslator
    {
        void IWorldTranslator.TranslateWorld(double x, double y, double z)
        {
            GL.Translate(x, y, z);
        }

        void IWorldTranslator.Store()
        {
            GL.PushMatrix();
        }

        void IWorldTranslator.Reset()
        {
            GL.PopMatrix();
        }
    }
}
