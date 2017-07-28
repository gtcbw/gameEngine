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
using World.Model;

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

            VectorHelper vectorHelper = new VectorHelper();

            BitmapToHeightConverter converter = new BitmapToHeightConverter();
            float[] heightValues = converter.ConvertBitmap("heightmap.bmp", 20);
            int sideLength = 500;
            int meters = 2;
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, sideLength, meters);

            PlayerPositionProvider playerPositionProvider = new PlayerPositionProvider(pressedKeyDetector, 
                heightCalculator, 
                timeProvider, 
                playerViewDirectionProvider,
                vectorHelper,
                new KeyMapper(pressedKeyDetector),
                3,
                500,
                500);

            ICamera camera = new Camera(config.Resolution.AspectRatio, playerPositionProvider);

            // environment rendring
            ITexture horizontexture = textureCache.LoadTexture("horizon.bmp");
            IEnumerable<Polygon> polygons = new SurfaceRectangleBuilder().CreateRectangle(-1, 0, 4, 1);
            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITextureTranslator textureTranslator = new TextureTranslator();
            IWorldTranslator worldTranslator = new WorldTranslator();
            IWorldRotator worldRotator = new WorldRotator();
            IRenderingElement horizon = new Horizon(horizontexture, textureChanger, polygonRenderer, polygons, playerViewDirectionProvider, textureTranslator, worldTranslator);
            IColorSetter colorSetter = new ColorSetter();

            IBufferObjectFactory bufferObjectFactory = new BufferObjectFactory();
            IBufferedMeshUnitRenderer bufferedMeshUnitRenderer = new BufferedMeshUnitRenderer();
            IMeshUnitCollection floorCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection streetCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IDelayedFloorLoader floorLoader = new DelayedFloorLoader(bufferObjectFactory, heightCalculator, floorCollection, 50, 2);
            FieldManager fieldManager = new FieldManager(playerPositionProvider, floorLoader, new FieldChangeAnalyzer(), new ActiveFieldCalculator(100, 10));

            IFog fog = new Fog();
            float[] color = { (float)(1.0 / 255.0 * 50.0), (float)(1.0 / 255.0 * 150.0), (float)(1.0 / 255.0 * 50.0) };
            fog.SetColor(color);

            CurvedStreetMeshBuilder curvedStreetMeshBuilder = new CurvedStreetMeshBuilder(vectorHelper, 
                heightCalculator,
                bufferObjectFactory,
                8,
                100);
            ITexture streettexture = textureCache.LoadTexture("street.bmp");
            streetCollection.AddMeshUnit(0, curvedStreetMeshBuilder.BuildMeshUnit(new Position { X = 500, Z = 500 }, 0, 90));
            streetCollection.AddMeshUnit(1, curvedStreetMeshBuilder.BuildMeshUnit(new Position { X = 500, Z = 500 }, 90, 180));
            streetCollection.AddMeshUnit(2, curvedStreetMeshBuilder.BuildMeshUnit(new Position { X = 500, Z = 500 }, 180, 270));
            streetCollection.AddMeshUnit(3, curvedStreetMeshBuilder.BuildMeshUnit(new Position { X = 500, Z = 500 }, 270, 360));

            IRenderingElement colorRenderer = new ColorRenderer((IRenderingElement)floorCollection, colorSetter);

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
                    colorRenderer.Render();
                    textureChanger.SetTexture(streettexture.TextureId);
                    ((IRenderingElement)streetCollection).Render();
                    fog.StopFog();

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
