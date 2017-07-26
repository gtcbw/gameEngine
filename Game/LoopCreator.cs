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


            BitmapToHeightConverter converter = new BitmapToHeightConverter();
            float[] heightValues = converter.ConvertBitmap("heightmap.bmp", 20);
            int sideLength = 500;
            int meters = 2;
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            PlayerPositionProvider playerPositionProvider = new PlayerPositionProvider(pressedKeyDetector, 
                heightCalculator, 
                timeProvider, 
                playerViewDirectionProvider, 
                new VectorHelper(),
                new KeyMapper(pressedKeyDetector),
                3,
                500,
                500);

            ICamera camera = new Camera(4.0 / 3.0, playerPositionProvider);

            // environment rendring
            ITexture horizontexture = textureCache.LoadTexture("horizon.bmp");
            IEnumerable<Polygon> polygons = new SurfaceRectangleBuilder().CreateRectangle(-1, 0, 4, 1);
            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITextureTranslator textureTranslator = new TextureTranslator();
            IWorldTranslator worldTranslator = new WorldTranslator();
            IWorldRotator worldRotator = new WorldRotator();
            IRenderingElement horizon = new Horizon(horizontexture, textureChanger, polygonRenderer, polygons, playerViewDirectionProvider, textureTranslator, worldTranslator);


            IBufferObjectFactory bufferObjectFactory = new BufferObjectFactory();
            IBufferedMeshUnitRenderer bufferedMeshUnitRenderer = new BufferedMeshUnitRenderer();
            FloorCollection floorCollection = new FloorCollection(bufferedMeshUnitRenderer);
            IDelayedFloorLoader floorLoader = new DelayedFloorLoader(bufferObjectFactory, heightCalculator, floorCollection, 50, 2);
            FieldManager fieldManager = new FieldManager(playerPositionProvider, floorLoader, new FieldChangeAnalyzer(), new ActiveFieldCalculator(100, 10));

            IFog fog = new Fog();
            float[] color = { 0.3f, 0.7f, 0.3f };
            fog.SetColor(color);

            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    screenClearer.CleanScreen();

                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();
                    playerPositionProvider.UpdatePosition();
                    fieldManager.UpdateFieldsByPlayerPosition();
                    // calculation

                    //rendring 2D
                    camera.SetDefaultPerspective();
                    horizon.Render();

                    //rendering 3D
                    camera.SetInGamePerspective();

                    //render floor
                    fog.StartFog();
                    ((IRenderingElement)floorCollection).Render();
                    fog.StopFog();

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
