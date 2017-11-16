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

            MousePositionController mousePositionController = new MousePositionController(window.Mouse, config.InvertMouse, config.MouseSensitivity);
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

            BitmapToHeightConverter converter = new BitmapToHeightConverter();
            float[] heightValues = converter.ConvertBitmap("heightmap.bmp", 5);
            bool[][] plantmap = new PlantGridReader().ConvertBitmapToGridByColor("plantmap.bmp", 255, 255, 255);
            IPositionFilter filter = new PlantGrid(plantmap, 10);
            int numberOfQuadsPerSideOfArea = 150;
            int metersPerQuad = 2;
            int numberOfFieldsPerAreaSide = 3;
            int lengthOfFieldSide = 100;

            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITranslator worldTranslator = new Translator();
            IWorldRotator worldRotator = new WorldRotator();
            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, numberOfQuadsPerSideOfArea, metersPerQuad);
            IBufferObjectFactory bufferObjectFactory = new BufferObjectFactory();
            IModelRepository modelLoader = new ModelCache(new ModelLoader("models", textureCache, bufferObjectFactory));
            IVertexBufferUnitRenderer bufferedMeshUnitRenderer = new VertexBufferUnitRenderer();
            IModelInstanceRenderer modelInstanceRenderer = new ModelInstanceRenderer(textureChanger, bufferedMeshUnitRenderer, worldTranslator, worldRotator, matrixManager);
            ModelContainer modelContainer = new ModelContainer(modelLoader, modelInstanceRenderer);
            

            PlayerMotionEncapsulator playerMotionEncapsulator = new PlayerMotionEncapsulator(vectorHelper, new World.Model.Position { X = 84, Y = 0, Z = 85 });

            ITexture bike = textureCache.LoadTexture("bike.png");
            IEnumerable<Polygon> bikeShape = surfaceRectangleBuilder.CreateRectangle(-0.5, 0.5, 4, 4, z: 0);

            IVehicleRepository vehicleRepository = new VehicleRepository(heightCalculator);

            PositionDistanceComparer positionDistanceComparer = new PositionDistanceComparer();
            VehicleManager vehicleManager = new VehicleManager(vehicleRepository.GetAllVehicles(), playerMotionEncapsulator,
                new SpriteRenderer(new TextureRenderer(new PolygonListRenderer(bikeShape, polygonRenderer), bike, textureChanger), worldTranslator, playerMotionEncapsulator, worldRotator, matrixManager),
                positionDistanceComparer,
                lengthOfFieldSide);
            IPositionRotator positionRotator = new PositionRotator();

            ICuboidWithWorldTester cuboidWithWorldTester = new CuboidWithWorldTester
                (new CuboidWithModelsTester(new CuboidCollisionTester(), positionRotator), 
                new CollisionModelCache(modelContainer, playerMotionEncapsulator, positionDistanceComparer, 15, 16),
                vehicleManager);

            ITexture bikeInHands = textureCache.LoadTexture("bikeScreen.png");
            IEnumerable<Polygon> bikeScreen = surfaceRectangleBuilder.CreateRectangle(0, 0, 1, 0.5f);
            VehicleUsageRenderer vehicleUsageObserver = new VehicleUsageRenderer(new TextureRenderer(new PolygonListRenderer(bikeScreen, polygonRenderer), bikeInHands, textureChanger), 
                worldTranslator, matrixManager, new PercentProvider(timeProvider, 0.7), new PercentProvider(timeProvider, 0.3));

            PlayerMotionManager playerMotionManager = new PlayerMotionManager(playerMotionEncapsulator,
                new WalkPositionCalculator(heightCalculator, timeProvider, vectorHelper, mousePositionController, new KeyMapper(pressedKeyDetector), 30),
                cuboidWithWorldTester,
                new PressedKeyEncapsulator(Keys.Enter, pressedKeyDetector),
                new VehicleMotionCalculator(vectorHelper, mousePositionController, new KeyMapper(pressedKeyDetector), heightCalculator, timeProvider),
                new ReboundCalculator(vectorHelper, mousePositionController, heightCalculator, timeProvider),
                timeProvider,
                vehicleManager,
                new VehicleUpClimber(new PercentProvider(timeProvider, 1.0), new PercentProvider(timeProvider, 0.4), new PercentProvider(timeProvider, 0.6)),
                new VehicleDownClimber(new PercentProvider(timeProvider, 1.0), new PercentProvider(timeProvider, 0.6)),
                vehicleUsageObserver,
                new VehicleExitPositionFinder(3, vectorHelper, cuboidWithWorldTester, heightCalculator));

            ICamera camera = new Camera(config.Resolution.AspectRatio, playerMotionEncapsulator);

            // environment rendring
            ITexture horizontexture = textureCache.LoadTexture("jungle.png");
            IEnumerable<Polygon> polygons = surfaceRectangleBuilder.CreateRectangle(-1, 0, 4, 1);
            
            IRenderingElement horizon = new Horizon(horizontexture, textureChanger, polygonRenderer, polygons, playerMotionEncapsulator, worldTranslator, matrixManager);
            IColorSetter colorSetter = new ColorSetter();

            IMeshUnitCollection floorCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection streetCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection treeCollection = new MeshUnitCollection
                (new VertexBufferUnitOffsetRenderer(8, new IndexFactorByViewDirectionProvider(playerMotionEncapsulator)));

            IMeshUnitCollection tree2Collection = new MeshUnitCollection
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

            IPositionGenerator treePositionGenerator = new PositionGenerator(filter, 
                new BoolProvider(new [] { false, true, false, false, true, false, true, false, false, false, true}), 
                heightCalculator, 
                lengthOfFieldSide, 
                6);
            IVertexByFieldCreator treeCreator = new TreeVertexCreator(new TreePrototypeProvider(vectorHelper)
                .GetPrototype(16, 16), treePositionGenerator);
            IMeshUnitCreator treeMeshUnitCreator = new TreeMeshUnitCreator(bufferObjectFactory);
            IFieldChangeObserver treeLoader = new FrameDelayUnitByFieldLoader(
                new DelayedMeshUnitLoader(treeCreator,
                treeMeshUnitCreator,
                treeCollection), 12);

            ////////////////// redundant tree 2
            IPositionGenerator tree2PositionGenerator = new PositionGenerator(filter,
                new BoolProvider(new[] { true, false, true, true, false, true, false, true, false, true, false }),
                heightCalculator,
                lengthOfFieldSide,
                8);
            IVertexByFieldCreator tree2Creator = new TreeVertexCreator(new TreePrototypeProvider(vectorHelper)
               .GetPrototype(8, 4), tree2PositionGenerator);
            IFieldChangeObserver tree2Loader = new FrameDelayUnitByFieldLoader(
                new DelayedMeshUnitLoader(tree2Creator,
                treeMeshUnitCreator,
                tree2Collection), 18);
            ///////////////////////////////


            //model
            ModelQueue modelQueue = new ModelQueue(modelLoader, modelContainer);
            IFieldModelLoader fieldModelLoader = new FieldModelLoader(heightCalculator);
            IFieldChangeObserver modelQueuePusher = new FrameDelayUnitByFieldLoader(new ModelQueuePusher(fieldModelLoader, modelQueue), 18);


            FieldManager fieldManager = new FieldManager(playerMotionEncapsulator,
                new List<IFieldChangeObserver> { floorLoader, streetLoader, treeLoader, tree2Loader, modelQueuePusher },
                new FieldChangeAnalyzer(),
                new ActiveFieldCalculator(lengthOfFieldSide, numberOfFieldsPerAreaSide));

            IFog fog = new Fog();
            float[] color = { (float)(1.0 / 255.0 * 0.0), (float)(1.0 / 255.0 * 255.0), (float)(1.0 / 255.0 * 0.0) };
            fog.SetColor(color);

            ITexture streettexture = textureCache.LoadTexture("street.bmp");
            ITexture treetexture = textureCache.LoadTexture("tree.png", true);
            ITexture treetexture2 = textureCache.LoadTexture("tree2.png", true);

            IRenderingElement colorRenderer = new ColorRenderer((IRenderingElement)floorCollection, colorSetter);
           

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
            IRayWithModelsTester rayWithModelsTester = new RayWithModelsTester(rayWithFacesTester, positionDistanceTester, obtuseAngleTester, positionRotator);
            RayWithWorldTester rayWithWorldTester = new RayWithWorldTester(rayWithMapTester, rayWithModelsTester, modelContainer, vehicleManager);
            ParticleContainer particleContainer = new ParticleContainer(timeProvider, worldTranslator, matrixManager, textureChanger, treetexture, polygonRenderer, 
                surfaceRectangleBuilder.CreateRectangle(0.2, 0.5, 0.6f, 0.6f, z:0), playerMotionEncapsulator, worldRotator);
            RayTrigger rayTrigger = new RayTrigger(rayWithWorldTester, playerMotionEncapsulator, mouseButtonEventProvider, particleContainer);
            //

            IFontMapper fontMapper = new FontMapper(textureCache, "font");
            IFontRenderer fontRenderer = new FontRenderer(surfaceRectangleBuilder.CreateRectangle(0, 0, 0.025f, 0.025f), polygonRenderer, worldTranslator, textureChanger, matrixManager,  0.03);
            FrameCounter frameCounter = new FrameCounter(fontMapper, fontRenderer);


            //target cross
            ITexture cross = textureCache.LoadTexture("cross.png");
            IEnumerable<Polygon> crossShape = surfaceRectangleBuilder.CreateRectangle(0.45, 0.45, 0.1f, 0.1f);
            //hell in my hands
            ITexture gunscreentexture = textureCache.LoadTexture("gunscreen.png");
            IEnumerable<Polygon> gunScreenShape = surfaceRectangleBuilder.CreateRectangle(-0.2, 0.0, 1, 0.5f);
            //
            IRenderingElement layerAlphaRenderer = new AlphaTestRenderer(new ListRenderer(new List<IRenderingElement>
            {
                 vehicleUsageObserver,
                new TextureRenderer(new PolygonListRenderer(crossShape, polygonRenderer), cross, textureChanger),
                //new TextureRenderer(new PolygonListRenderer(gunScreenShape, polygonRenderer), gunscreentexture, textureChanger)
                }), new AlphaTester());

            ScreenshotMaker screenshotMaker = new ScreenshotMaker("C:\\screenshots\\", 100, 0.04, config.Resolution.X, config.Resolution.Y, pressedKeyDetector, timeProvider);

            //enemy
            IAnimated360DegreeTextureLoader animated360DegreeTextureLoader = new Animated360DegreeTextureLoader(textureCache);
            ITextureByAnimationPercentSelector textureByAnimationPercentSelector = new TextureByAnimationPercentSelector();
            ITextureSequenceSelector textureSequenceSelector = new TextureSequenceSelector();

            IEnumerable<Polygon> footShape = surfaceRectangleBuilder.CreateRectangle(-0.5, 0.5, 2, 1, z: 0);
            var footWalkAnimation = animated360DegreeTextureLoader.LoadAnimatedTexture("characters\\feet\\walk forward");

            IEnumerable<Polygon> torsoShape = surfaceRectangleBuilder.CreateRectangle(0, 0.5, 1, 1, z: 0);
            var torsoAnimation = animated360DegreeTextureLoader.LoadAnimatedTexture("characters\\torso");

            IEnumerable<Polygon> headShape = surfaceRectangleBuilder.CreateRectangle(-0.5, 0.5, 2, 2, z: 0);
            var headAnimation = animated360DegreeTextureLoader.LoadAnimatedTexture("characters\\head");

           var gunModel = modelLoader.Load(new ModelInstanceDescription { Filename = "gun hands.mod", Position = new World.Model.Position() });

            IRenderedRotationCalculator renderedRotationCalculator = new RenderedRotationCalculator(playerMotionEncapsulator);
            Animation360DegreeRenderer animation360DegreeRenderer = new Animation360DegreeRenderer(textureByAnimationPercentSelector, textureSequenceSelector, textureChanger, 
                new PercentProvider(timeProvider, 1.0), footWalkAnimation, torsoAnimation, headAnimation, gunModel, modelInstanceRenderer, renderedRotationCalculator, matrixManager, 
                new PolygonListRenderer(footShape, polygonRenderer), new PolygonListRenderer(torsoShape, polygonRenderer), new PolygonListRenderer(headShape, polygonRenderer),
                worldTranslator, playerMotionEncapsulator, worldRotator, timeProvider, heightCalculator, playerMotionEncapsulator, new RotationCalculator());

            IRenderingElement tree1 = new TextureRenderer((IRenderingElement)treeCollection, treetexture, textureChanger);
            IRenderingElement tree2 = new TextureRenderer((IRenderingElement)tree2Collection, treetexture2, textureChanger);

            IRenderingElement cullingDeactivator = new CullingDeactivator(new CullingController(), 
                new AlphaTestRenderer(new ListRenderer(new List<IRenderingElement> { tree1, tree2, vehicleManager, animation360DegreeRenderer, particleContainer }), new AlphaTester()));

            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    screenClearer.CleanScreen();

                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();
                    playerMotionManager.CalculatePlayerMotion();
                    vehicleManager.UpdateVehicles();
                    fieldManager.UpdateFieldsByPlayerPosition();

                    //rendering 2D
                    camera.SetDefaultPerspective();
                    horizon.Render();
                   

                    //rendering 3D
                    camera.SetInGamePerspective();

                    //fog.StartFog();
                    colorRenderer.Render();
                    textureChanger.SetTexture(streettexture.TextureId);
                    ((IRenderingElement)streetCollection).Render();

                    cullingDeactivator.Render();

                    light.Enable();
                    ((IRenderingElement)modelContainer).Render();
                    light.Disable();

                    //fog.StopFog();

                    rayTrigger.DoStuff();

                    //rendering final 2D layer
                    camera.SetDefaultPerspective();
                    layerAlphaRenderer.Render();
                    frameCounter.MeasureAndRenderFramesPerSecond();

                    // screenshot
                    screenshotMaker.ExecuteLogic();

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
