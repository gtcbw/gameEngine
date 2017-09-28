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

namespace Game
{
    internal class LoopCreator
    {
        internal static Action BuildLoop(Configuration config)
        {
            Window window = new Window(config.Resolution.X, config.Resolution.Y);
            window.VSync =  OpenTK.VSyncMode.Off;

            IMousePositionController mousePositionController = new MousePositionController(window.Mouse, config.InvertMouse, config.MouseSensitivity);
            IMouseButtonEventProvider mouseButtonEventProvider = new MouseButtonEventProvider(window.Mouse);
            IPressedKeyDetector pressedKeyDetector = new PressedKeyDetector(window.Keyboard);
            FrameTimeProvider timeProvider = new FrameTimeProvider();
            IScreenClearer screenClearer = new ScreenClearer();
            ITextureChanger textureChanger = new TextureChanger();
            ITextureLoader textureCache = new TextureCache(new TextureLoader(textureChanger, "textures"));
            BufferLoader bufferLoader = new BufferLoader(new WavFileReader());
            ISoundFactory soundFactory = new SoundFactory(bufferLoader, 100, config.SoundVolume / 100.0f);
            SurfaceRectangleBuilder surfaceRectangleBuilder = new SurfaceRectangleBuilder();

           
            VectorHelper vectorHelper = new VectorHelper();

            BitmapToHeightConverter converter = new BitmapToHeightConverter();
            float[] heightValues = converter.ConvertBitmap("heightmap.bmp", 5);
            bool[][] plantmap = new PlantGridReader().ConvertBitmapToGridByColor("plantmap.bmp", 255, 255, 255);
            IPositionFilter filter = new PlantGrid(plantmap, 10);
            int numberOfQuadsPerSideOfArea = 150;
            int metersPerQuad = 2;
            int numberOfFieldsPerAreaSide = 3;
            int lengthOfFieldSide = 100;

            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITextureTranslator textureTranslator = new TextureTranslator();
            IWorldTranslator worldTranslator = new WorldTranslator();
            IWorldRotator worldRotator = new WorldRotator();
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, numberOfQuadsPerSideOfArea, metersPerQuad);
            IBufferObjectFactory bufferObjectFactory = new BufferObjectFactory();
            IModelRepository modelLoader = new ModelRepository("models", textureCache, bufferObjectFactory);
            IVertexBufferUnitRenderer bufferedMeshUnitRenderer = new VertexBufferUnitRenderer();
            ModelContainer modelContainer = new ModelContainer(modelLoader, textureChanger, bufferedMeshUnitRenderer, worldTranslator);
            

            PlayerMotionEncapsulator playerMotionEncapsulator = new PlayerMotionEncapsulator(vectorHelper, new World.Model.Position { X = 100, Y = 0, Z = 100 });

            ITexture bike = textureCache.LoadTexture("bike.png");
            IEnumerable<Polygon> bikeShape = surfaceRectangleBuilder.CreateRectangle(-0.5, 0.5, 2, 1, z: 0);

            IVehicleRepository vehicleRepository = new VehicleRepository(heightCalculator);

            VehicleProvider vehicleProvider = new VehicleProvider(vehicleRepository.GetAllVehicles(), playerMotionEncapsulator,
                new SpriteRenderer(new TextureRenderer(new PolygonListRenderer(bikeShape, polygonRenderer), bike, textureChanger), worldTranslator, playerMotionEncapsulator, worldRotator),
                new PositionDistanceComparer(),
                lengthOfFieldSide);
            ICuboidWithWorldTester cuboidWithWorldTester = new CuboidWithWorldTester(new CuboidWithModelsTester(new CuboidCollisionTester()), modelContainer, vehicleProvider);

            PlayerMotionManager playerMotionManager = new PlayerMotionManager(playerMotionEncapsulator,
                new WalkPositionCalculator(heightCalculator, timeProvider, vectorHelper, mousePositionController, new KeyMapper(pressedKeyDetector), 30),
                cuboidWithWorldTester,
                new PressedKeyEncapsulator(Keys.Enter, pressedKeyDetector),
                new VehicleMotionCalculator(vectorHelper, mousePositionController, new KeyMapper(pressedKeyDetector), heightCalculator, timeProvider),
                new ReboundCalculator(vectorHelper, mousePositionController, heightCalculator, timeProvider),
                timeProvider,
                new VehicleFinder(vehicleProvider, new PositionDistanceComparer()),
                new VehicleUpClimber(new PercentProvider(timeProvider, 1.0), new PercentProvider(timeProvider, 0.4), new PercentProvider(timeProvider, 0.6)),
                new VehicleDownClimber(new PercentProvider(timeProvider, 1.0), new PercentProvider(timeProvider, 0.6)));

            ICamera camera = new Camera(config.Resolution.AspectRatio, playerMotionEncapsulator);

            // environment rendring
            ITexture horizontexture = textureCache.LoadTexture("horizon.bmp");
            IEnumerable<Polygon> polygons = surfaceRectangleBuilder.CreateRectangle(-1, 0, 4, 1);
            
            IRenderingElement horizon = new Horizon(horizontexture, textureChanger, polygonRenderer, polygons, playerMotionEncapsulator, textureTranslator, worldTranslator);
            IColorSetter colorSetter = new ColorSetter();

            IMeshUnitCollection floorCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection streetCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection treeCollection = new MeshUnitCollection
                (new VertexBufferUnitOffsetRenderer(8, new IndexFactorByViewDirectionProvider(playerMotionEncapsulator)));

            IVertexByFieldCreator floorVertexCreator = new FloorVertexCreator(heightCalculator, lengthOfFieldSide / metersPerQuad, metersPerQuad);
            IMeshUnitCreator floorMeshUnitCreator = new FloorMeshUnitCreator(bufferObjectFactory, lengthOfFieldSide / metersPerQuad);
            IFieldChangeObserver floorLoader = new DelayedMeshUnitLoader(floorVertexCreator, floorMeshUnitCreator, floorCollection);

            IVertexByFieldCreator streetVertexCreator = new StreetVertexCreator(vectorHelper, heightCalculator, 8, lengthOfFieldSide / 2.0);
            IMeshUnitCreator streetMeshUnitCreator = new StreetMeshUnitCreator(bufferObjectFactory, 90);
            IFieldChangeObserver streetLoader = new FrameDelayUnitByFieldLoader(
                new DelayedMeshUnitLoader(streetVertexCreator, 
                streetMeshUnitCreator, 
                streetCollection), 6);

            IPositionGenerator positionGenerator = new PositionGenerator(filter, 
                new BoolProvider(new [] { false, true, false, false, true, false, true, false, false, false, true}), 
                heightCalculator, 
                lengthOfFieldSide, 
                10);
            IVertexByFieldCreator treeCreator = new TreeVertexCreator(new TreePrototypeProvider(vectorHelper)
                .GetPrototype(3, 16), positionGenerator);
            IMeshUnitCreator treeMeshUnitCreator = new TreeMeshUnitCreator(bufferObjectFactory);
            IFieldChangeObserver treeLoader = new FrameDelayUnitByFieldLoader(
                new DelayedMeshUnitLoader(treeCreator,
                treeMeshUnitCreator,
                treeCollection), 12);

            //model
            ModelQueue modelQueue = new ModelQueue(modelLoader, modelContainer);
            IFieldModelLoader fieldModelLoader = new FieldModelLoader(heightCalculator);
            IFieldChangeObserver modelQueuePusher = new FrameDelayUnitByFieldLoader(new ModelQueuePusher(fieldModelLoader, modelQueue), 18);


            FieldManager fieldManager = new FieldManager(playerMotionEncapsulator,
                new List<IFieldChangeObserver> { floorLoader, streetLoader, treeLoader, modelQueuePusher },
                new FieldChangeAnalyzer(),
                new ActiveFieldCalculator(lengthOfFieldSide, numberOfFieldsPerAreaSide));

            IFog fog = new Fog();
            float[] color = { (float)(1.0 / 255.0 * 50.0), (float)(1.0 / 255.0 * 150.0), (float)(1.0 / 255.0 * 50.0) };
            fog.SetColor(color);

            ITexture streettexture = textureCache.LoadTexture("street.bmp");
            ITexture treetexture = textureCache.LoadTexture("tree.png", true);

            IRenderingElement colorRenderer = new ColorRenderer((IRenderingElement)floorCollection, colorSetter);
            IRenderingElement alphaRenderer = new AlphaTestRenderer((IRenderingElement)treeCollection, new AlphaTester());

            //light
            ILightCollectionProvider lightCollectionProvider = new LightCollectionProvider();
            ILightCollection light = lightCollectionProvider.GetCollection();
            //

            //ray test
            IRayWithMapTester rayWithMapTester = new RayWithMapTester(heightCalculator, 120);
            IPositionDistanceTester positionDistanceTester = new PositionDistanceTester();
            IIntersectionCalculator intersectionCalculator = new IntersectionCalculatorWithFaceCulling();
            IObtuseAngleTester obtuseAngleTester = new ObtuseAngleTester();
            IRayWithFacesTester rayWithFacesTester = new RayWithFacesTester(intersectionCalculator, obtuseAngleTester, positionDistanceTester);
            IRayWithModelsTester rayWithModelsTester = new RayWithModelsTester(rayWithFacesTester, positionDistanceTester, obtuseAngleTester);
            RayWithWorldTester rayWithWorldTester = new RayWithWorldTester(rayWithMapTester, rayWithModelsTester, modelContainer, vehicleProvider);
            ParticleContainer particleContainer = new ParticleContainer(timeProvider, worldTranslator, textureChanger, treetexture, polygonRenderer, 
                surfaceRectangleBuilder.CreateRectangle(0.2, 0.5, 0.6f, 0.6f, z:0), playerMotionEncapsulator, worldRotator);
            RayTrigger rayTrigger = new RayTrigger(rayWithWorldTester, playerMotionEncapsulator, mouseButtonEventProvider, particleContainer);
            //

            IFontMapper fontMapper = new FontMapper(textureCache, "font");
            IFontRenderer fontRenderer = new FontRenderer(surfaceRectangleBuilder.CreateRectangle(0, 0, 0.025f, 0.025f), polygonRenderer, worldTranslator, textureChanger, 0.03);
            FrameCounter frameCounter = new FrameCounter(fontMapper, fontRenderer);


            //target cross
            ITexture cross = textureCache.LoadTexture("cross.png");
            IEnumerable<Polygon> crossShape = surfaceRectangleBuilder.CreateRectangle(0.45, 0.45, 0.1f, 0.1f);
            IRenderingElement layerAlphaRenderer = new AlphaTestRenderer(new TextureRenderer(new PolygonListRenderer(crossShape, polygonRenderer), cross, textureChanger), new AlphaTester());

            ScreenshotMaker screenshotMaker = new ScreenshotMaker("C:\\screenshots\\", 60, 0.05, config.Resolution.X, config.Resolution.Y, pressedKeyDetector, timeProvider);
            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    screenClearer.CleanScreen();

                    timeProvider.MeasureTimeSinceLastFrame();
                    playerMotionManager.CalculatePlayerMotion();
                    vehicleProvider.UpdateVehicles();
                    fieldManager.UpdateFieldsByPlayerPosition();

                    //rendering 2D
                    camera.SetDefaultPerspective();
                    horizon.Render();
                    frameCounter.MeasureAndRenderFramesPerSecond();

                    //rendering 3D
                    camera.SetInGamePerspective();

                    //fog.StartFog();
                    colorRenderer.Render();
                    textureChanger.SetTexture(streettexture.TextureId);
                    ((IRenderingElement)streetCollection).Render();
                    textureChanger.SetTexture(treetexture.TextureId);
                    alphaRenderer.Render();

                    ((IRenderingElement)vehicleProvider).Render();

                    light.Enable();
                    ((IRenderingElement)modelContainer).Render();
                    light.Disable();

                    ((IRenderingElement)particleContainer).Render();
                    //fog.StopFog();

                    rayTrigger.DoStuff();

                    //rendering final 2D layer
                    camera.SetDefaultPerspective();
                    layerAlphaRenderer.Render();

                    // screenshot
                    screenshotMaker.ExecuteLogic();

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
