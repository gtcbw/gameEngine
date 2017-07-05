using Engine.Framework;
using Graphics;
using Graphics.Contracts;
using System;

namespace Game
{
    public class LoopCreator
    {
        public static Action BuildLoop()
        {
            FrameTimeProvider frameTimeProvider = new FrameTimeProvider();
            IScreenClearer screenClearer = new ScreenClearer();

            return () =>
            {
                frameTimeProvider.MeasureTimeSinceLastFrame();

                screenClearer.CleanScreen();
            };
        }
    }
}
