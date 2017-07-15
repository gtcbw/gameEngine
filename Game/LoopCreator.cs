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

            IPlayerPositionProvider playerPositionProvider = new PlayerPositionProvider();
            IPlayerViewRayProvider playerViewRayProvider = new PlayerViewRayProvider(playerPositionProvider, playerViewDirectionProvider, new VectorHelper());
            ICamera camera = new Camera(4.0 / 3.0, playerViewRayProvider);

            // environment rendring
            ITexture horizontexture = textureCache.LoadTexture("gebirgedunkel.bmp");
            IEnumerable<Polygon> polygons = new SurfaceRectangleBuilder().CreateRectangle(-1, 0, 4, 1);
            IPolygonRenderer polygonRenderer = new PolygonRenderer();
            ITextureTranslator textureTranslator = new TextureTranslator();
            IWorldTranslator worldTranslator = new WorldTranslator();
            IWorldRotator worldRotator = new WorldRotator();
            IRenderingElement horizon = new Horizon(horizontexture, textureChanger, polygonRenderer, polygons, playerViewDirectionProvider, textureTranslator, worldTranslator);

            // tempboden 
            ITexture boden = textureCache.LoadTexture("boden.bmp");
            IEnumerable<Polygon> floorPolygons = new SurfaceRectangleBuilder().CreateRectangle(-100, -100, 200, 200, 0, 400);
            //

            // height
            int sideLength = 10;
            int meters = 2;
            float[] heightValues = new float[sideLength * sideLength];
            for (int i = 0; i < sideLength * sideLength; i++)
            {
                heightValues[i] = (float)((i % 10) / 2.0);
            }
            IHeightCalculator HeightCalculator = new HeightCalculator(heightValues, sideLength, meters);
            IBufferedMeshUnitFactory bufferedMeshUnitFactory = new BufferedMeshUnitFactory();
            BufferedMeshUnit unit = CreateBuffer(bufferedMeshUnitFactory);
            IBufferedMeshUnitRenderer bufferedMeshUnitRenderer = new BufferedMeshUnitRenderer();
            //

            return () =>
            {
                while(!pressedKeyDetector.IsKeyDown(Keys.Escape))
                {
                    timeProvider.MeasureTimeSinceLastFrame();
                    mousePositionController.MeasureMousePositionDelta();
                    screenClearer.CleanScreen();


                    // calculation

                    //rendring 2D
                    camera.SetDefaultPerspective();
                    horizon.Render();

                    //rendering 3D
                    camera.SetInGamePerspective();
                    //textureChanger.SetTexture(boden.TextureId);
                    //worldTranslator.Store();
                    //worldRotator.RotateX(90);
                    //polygonRenderer.RenderPolygons(floorPolygons);
                    //worldTranslator.Reset();

                    //render floor
                    bufferedMeshUnitRenderer.RenderMesh(unit);

                    ((IBufferSwapper)window).SwapBuffers();
                }
            };
        }

        public static BufferedMeshUnit CreateBuffer(IBufferedMeshUnitFactory bufferedMeshUnitFactory)
        {

            float[] array = new float[4 * 3];

            array[0] = -5;
            array[1] = 0;
            array[2] = -5;

            array[3] = 5;
            array[4] = 0;
            array[5] = -5;

            array[6] = 5;
            array[7] = 0;
            array[8] = 5;

            array[9] = -5;
            array[10] = 0;
            array[11] = 5;

            ushort[] indices = new ushort[6];

            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;

            return bufferedMeshUnitFactory.GenerateBuffer(array, indices);
        }
    }
}
