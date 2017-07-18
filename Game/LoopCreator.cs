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

namespace Game
{
    internal class LoopCreator
    {
        internal static Action BuildLoop(Configuration config)
        {
            Window window = new Window(config.Resolution.X, config.Resolution.Y);

            PlayerViewDirectionProvider playerViewDirectionProvider = new PlayerViewDirectionProvider(config.InvertMouse, config.MouseSensitivity, 80.0);
            IMousePositionController mousePositionController = new MousePositionController(window.Mouse, playerViewDirectionProvider);
            IMouseButtonEventProvider mouseButtonEventProvider = new MouseButtonEventProvider(window.Mouse);
            IPressedKeyDetector pressedKeyDetector = new PressedKeyDetector(window.Keyboard);
            FrameTimeProvider timeProvider = new FrameTimeProvider();
            IScreenClearer screenClearer = new ScreenClearer();
            ITextureChanger textureChanger = new TextureChanger();
            ITextureLoader textureCache = new TextureCache(new TextureLoader(textureChanger));
            BufferLoader bufferLoader = new BufferLoader(new WavFileReader());
            ISoundFactory soundFactory = new SoundFactory(bufferLoader, 100, config.SoundVolume / 100.0f);

            // height
            int sideLength = 10;
            int meters = 2;
            float[] heightValues = new float[sideLength * sideLength];
            for (int i = 0; i < sideLength * sideLength; i++)
            {
                heightValues[i] = (float)((i % 3) / 3.0);
            }
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            PlayerPositionProvider playerPositionProvider = new PlayerPositionProvider(pressedKeyDetector, 
                heightCalculator, 
                timeProvider, 
                playerViewDirectionProvider, 
                new VectorHelper(),
                new KeyMapper(pressedKeyDetector));
            //IPlayerViewRayProvider playerViewRayProvider = new PlayerViewRayProvider(playerPositionProvider, playerViewDirectionProvider, new VectorHelper());
            ICamera camera = new Camera(4.0 / 3.0, playerPositionProvider);

            // environment rendring
            ITexture horizontexture = textureCache.LoadTexture("gebirgedunkel.bmp");
            IEnumerable<Polygon> polygons = new SurfaceRectangleBuilder().CreateRectangle(-1, 0, 4, 1);
            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITextureTranslator textureTranslator = new TextureTranslator();
            IWorldTranslator worldTranslator = new WorldTranslator();
            IWorldRotator worldRotator = new WorldRotator();
            IRenderingElement horizon = new Horizon(horizontexture, textureChanger, polygonRenderer, polygons, playerViewDirectionProvider, textureTranslator, worldTranslator);


            IBufferObjectFactory bufferObjectFactory = new BufferObjectFactory();
            IBufferedMeshUnitRenderer bufferedMeshUnitRenderer = new BufferedMeshUnitRenderer();
            BufferedMeshUnit unit = new MeshUnitBuilder(bufferObjectFactory, heightCalculator).CreateRelativeHeightMapUnit(20, 1, 0, 0);
            //

            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    screenClearer.CleanScreen();

                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();
                    playerPositionProvider.UpdatePosition();

                    // calculation

                    //rendring 2D
                    camera.SetDefaultPerspective();
                    horizon.Render();

                    //rendering 3D
                    camera.SetInGamePerspective();

                    //render floor
                    bufferedMeshUnitRenderer.RenderMesh(unit);

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
