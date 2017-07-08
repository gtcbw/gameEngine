using Engine.Framework;
using Graphics;
using Graphics.Contracts;
using System;
using Engine.Contracts.Input;
using Game.OpenTkDependencies;
using Sound;
using Sound.Contracts;

namespace Game
{
    internal class LoopCreator
    {
        internal static Action BuildLoop(Configuration config)
        {
            Window window = new Window(config.Resolution.X, config.Resolution.Y);

            PlayerViewDirectionProvider PlayerViewDirectionProvider = new PlayerViewDirectionProvider(config.InvertMouse, config.MouseSensitivity, 80.0);
            IMousePositionController mousePositionController = new MousePositionController(window.Mouse, PlayerViewDirectionProvider);
            IMouseButtonEventProvider mouseButtonEventProvider = new MouseButtonEventProvider(window.Mouse);
            IPressedKeyDetector pressedKeyDetector = new PressedKeyDetector(window.Keyboard);
            FrameTimeProvider timeProvider = new FrameTimeProvider();
            IScreenClearer screenClearer = new ScreenClearer();
            ITextureChanger textureChanger = new TextureChanger();
            ITextureLoader textureCache = new TextureCache(new TextureLoader(textureChanger));
            BufferLoader bufferLoader = new BufferLoader(new WavFileReader());
            ISoundFactory soundFactory = new SoundFactory(bufferLoader, 100, config.SoundVolume / 100.0f);

            return () =>
            {
                int i = 0;
                while(i < 300)
                {
                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();
                    screenClearer.CleanScreen();
                    ((IBufferSwapper)window).SwapBuffers();

                    i++;
                }
            };
        }
    }
}
