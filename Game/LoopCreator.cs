using Engine.Framework;
using Graphics;
using Graphics.Contracts;
using System;
using Engine.Contracts.Input;
using Game.OpenTkDependencies;
using Sound;
using Sound.Contracts;
using Landscape.Rendering;
using Engine.Contracts;
using System.Collections.Generic;
using Math;
using Math.Contracts;
using System.Linq;
using Engine.Contracts.Models;
using Engine.Framework.PlayerMotion;
using Engine.Contracts.PlayerMotion;
using Engine.Contracts.Animation;
using Engine.Framework.Animation;
using Character.Animation;

namespace Game
{
    internal class LoopCreator
    {
        internal static Action BuildLoop(Configuration config)
        {
            Window window = new Window(config.Resolution.X, config.Resolution.Y);
            window.VSync =  OpenTK.VSyncMode.Off;

            MousePositionController mousePositionController = new MousePositionController(config.InvertMouse, config.MouseSensitivity);
            IMouseButtonEventProvider mouseButtonEventProvider = new MouseButtonEventProvider(window.Mouse);
            IPressedKeyDetector pressedKeyDetector = new PressedKeyDetector(window.Keyboard);
            FrameTimeProvider timeProvider = new FrameTimeProvider();
            IScreenClearer screenClearer = new ScreenClearer();
            ITextureChanger textureChanger = new TextureChanger();
            ITextureLoader textureCache = new TextureCache(new TextureLoader(textureChanger, "textures"));
            BufferLoader bufferLoader = new BufferLoader(new WavFileReader());
            ISoundFactory soundFactory = new SoundFactory(bufferLoader, 100, config.SoundVolume / 100.0f);
            SurfaceRectangleBuilder surfaceRectangleBuilder = new SurfaceRectangleBuilder();
            IMatrixManager matrixManager = new MatrixManager();
            VectorHelper vectorHelper = new VectorHelper();
            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITranslator worldTranslator = new Translator();
            IWorldRotator worldRotator = new WorldRotator();
            IBufferObjectFactory bufferObjectFactory = new BufferObjectFactory();
            IVertexBufferUnitRenderer bufferedMeshUnitRenderer = new VertexBufferUnitRenderer();

            ICamera camera = new Camera(config.Resolution.AspectRatio);

            // environment rendring
            IFontMapper fontMapper = new FontMapper(textureCache, "font");
            IFontRenderer fontRenderer = new FontRenderer(surfaceRectangleBuilder.CreateRectangle(0, 0, 0.1f, 0.1f), polygonRenderer, worldTranslator, textureChanger, matrixManager,  0.1);
            FrameCounter frameCounter = new FrameCounter(fontMapper, fontRenderer);

            ScreenshotMaker screenshotMaker = new ScreenshotMaker("C:\\screenshots\\", 100, 0.04, config.Resolution.X, config.Resolution.Y, pressedKeyDetector, timeProvider);

            /////////////framebuffer test
            ///
            IFrameBufferFactory f = new FrameBufferFactory();
            FrameBuffer fId = f.GenerateFrameBuffer(config.Resolution.X / 10, config.Resolution.Y / 10);

            IEnumerable<Polygon> finalScreenLeft = surfaceRectangleBuilder.CreateRectangle(-0.3, 0, 0.8f, 0.9f, textureYOne: 0, textureYZero: 1);
            IRenderingElement polygonListRendererLeft = new PolygonListRenderer(finalScreenLeft, polygonRenderer);
            float lengthx = (float)config.Resolution.X / (float)config.Resolution.Y;
            IEnumerable<Polygon> finalScreenRight = surfaceRectangleBuilder.CreateRectangle(-(lengthx - 1) / 2.0, 0, lengthx, 1.0f, textureYOne: 0, textureYZero: 1);
            IRenderingElement polygonListRendererRight = new PolygonListRenderer(finalScreenRight, polygonRenderer);
            /////////////shader
            ShaderFactory shaderFactory = new ShaderFactory();
            int programId = shaderFactory.CreateShaderProgram();
            ViewPortSetter viewPortSetter = new ViewPortSetter();

            //second texture
            ITexture caro = textureCache.LoadTexture("wall.bmp");
            //wann shader deleten????
            ///////////////////


            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();

                    //framebuffer füllen
                    f.SetFrameBuffer(fId.FrameBufferId);
                    viewPortSetter.SetViewport(1920 / 10, 1080 / 10);
                    screenClearer.CleanScreen();
                    //rendering 2D
                    camera.SetDefaultPerspective();

                    frameCounter.MeasureAndRenderFramesPerSecond();

                    //render final layer
                    f.UnbindFrameBuffer();
                    viewPortSetter.SetViewport(1920, 1080);
                    screenClearer.CleanScreen();

                    textureChanger.SetTexture(fId.TextureId, 0);
                    //polygonListRendererLeft.Render();

                    textureChanger.SetTexture(caro.TextureId, 1);
                    shaderFactory.ActivateShaderProgram(programId);
                    polygonListRendererRight.Render();
                    shaderFactory.DeactivateShaderProgram();

                    // screenshot
                    screenshotMaker.ExecuteLogic();

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
