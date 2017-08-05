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
            float[] heightValues = converter.ConvertBitmap("heightmap.bmp", 50);
            bool[][] plantmap = new PlantGridReader().ConvertBitmapToGridByColor("plantmap.bmp", 255, 255, 255);
            IPositionFilter filter = new PlantGrid(plantmap, 10);
            int numberOfQuadsPerSideOfArea = 500;
            int metersPerQuad = 2;
            int numberOfFieldsPerAreaSide = 10;
            int lengthOfFieldSide = 100;

            IHeightCalculator heightCalculator = new HeightCalculator(heightValues, numberOfQuadsPerSideOfArea, metersPerQuad);

            PlayerPositionProvider playerPositionProvider = new PlayerPositionProvider(pressedKeyDetector, 
                heightCalculator, 
                timeProvider, 
                playerViewDirectionProvider,
                vectorHelper,
                new KeyMapper(pressedKeyDetector),
                30,
                50,
                50);

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
            IVertexBufferUnitRenderer bufferedMeshUnitRenderer = new VertexBufferUnitRenderer();
            IMeshUnitCollection floorCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection streetCollection = new MeshUnitCollection(bufferedMeshUnitRenderer);
            IMeshUnitCollection treeCollection = new MeshUnitCollection
                (new VertexBufferUnitOffsetRenderer(8, new IndexFactorByViewDirectionProvider(playerViewDirectionProvider)));

            IVertexByFieldCreator floorVertexCreator = new FloorVertexCreator(heightCalculator, lengthOfFieldSide / metersPerQuad, metersPerQuad);
            IMeshUnitCreator floorMeshUnitCreator = new FloorMeshUnitCreator(bufferObjectFactory, lengthOfFieldSide / metersPerQuad);
            IMeshUnitByFieldLoader floorLoader = new DelayedMeshUnitLoader(floorVertexCreator, floorMeshUnitCreator, floorCollection);

            IVertexByFieldCreator streetVertexCreator = new StreetVertexCreator(vectorHelper, heightCalculator, 8, lengthOfFieldSide / 2.0);
            IMeshUnitCreator streetMeshUnitCreator = new StreetMeshUnitCreator(bufferObjectFactory, 90);
            IMeshUnitByFieldLoader streetLoader = new FrameDelayUnitByFieldLoader(
                new DelayedMeshUnitLoader(streetVertexCreator, 
                streetMeshUnitCreator, 
                streetCollection), 6);

            IPositionGenerator positionGenerator = new PositionGenerator(filter, 
                new BoolProvider(new [] { false, true, false, false, true, false, true, false, false, false, true}), 
                heightCalculator, 
                lengthOfFieldSide, 
                10);
            IVertexByFieldCreator treeCreator = new TreeVertexCreator(new TreePrototypeProvider(vectorHelper)
                .GetPrototype(4, 16), positionGenerator);
            IMeshUnitCreator treeMeshUnitCreator = new TreeMeshUnitCreator(bufferObjectFactory);
            IMeshUnitByFieldLoader treeLoader = new FrameDelayUnitByFieldLoader(
                new DelayedMeshUnitLoader(treeCreator,
                treeMeshUnitCreator,
                treeCollection), 12);

            FieldManager fieldManager = new FieldManager(playerPositionProvider,
                new List<IMeshUnitByFieldLoader> { floorLoader, streetLoader, treeLoader },
                new FieldChangeAnalyzer(), 
                new ActiveFieldCalculator(lengthOfFieldSide, numberOfFieldsPerAreaSide));

            IFog fog = new Fog();
            float[] color = { (float)(1.0 / 255.0 * 50.0), (float)(1.0 / 255.0 * 150.0), (float)(1.0 / 255.0 * 50.0) };
            fog.SetColor(color);

            ITexture streettexture = textureCache.LoadTexture("street.bmp");
            ITexture treetexture = textureCache.LoadTexture("tree.png", true);

            IRenderingElement colorRenderer = new ColorRenderer((IRenderingElement)floorCollection, colorSetter);
            IRenderingElement alphaRenderer = new AlphaTestRenderer((IRenderingElement)treeCollection, new AlphaTester());

            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    screenClearer.CleanScreen();

                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();
                    playerPositionProvider.UpdatePosition();
                    fieldManager.UpdateFieldsByPlayerPosition();

                    //rendring 2D
                    camera.SetDefaultPerspective();
                    horizon.Render();

                    //rendering 3D
                    camera.SetInGamePerspective();

                    //fog.StartFog();
                    colorRenderer.Render();
                    textureChanger.SetTexture(streettexture.TextureId);
                    ((IRenderingElement)streetCollection).Render();
                    textureChanger.SetTexture(treetexture.TextureId);
                    alphaRenderer.Render();
                    //fog.StopFog();

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }
    }
}
