using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameworkContracts;
using Settings;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using Render.Contracts;

namespace Render
{
    public class Screen : IScreen
    {
        public double AspectRatio { private set; get; }
        public Resolution CurrentResolution { private set; get; }
         
        private IGameWindow GameWindow { set; get; }
        private IResolutionProvider _resolutionProvider;

        public Screen(IGameWindow gameWindow, IResolutionProvider resolutionProvider)
        {
            GameWindow = gameWindow;
            _resolutionProvider = resolutionProvider;
        }

        public void ChangeResolution(Resolution desiredResolution)
        {
            if (!ResolutionIsValid(desiredResolution))
                return;

            AspectRatio = ((double)desiredResolution.X) / ((double)desiredResolution.Y);

            SetViewPort(desiredResolution);
            GameWindow.Size = new Size(desiredResolution.X, desiredResolution.Y);

            CurrentResolution = desiredResolution;
        }

        public void SetViewPort(Resolution desiredResolution)
        {
            GL.Viewport(0, 0, desiredResolution.X, desiredResolution.Y); 
        }

        private bool ResolutionIsValid(Resolution desiredResolution)
        {
            foreach (Resolution resolution in _resolutionProvider.GetSupportedResolutions())
            {
                if (resolution.X == desiredResolution.X && resolution.Y == resolution.Y)
                    return true;
            }
            return false;
        }
    }
}
