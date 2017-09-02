using Engine.Contracts;
using Engine.Contracts.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace Game
{
    public class ScreenshotMaker
    {
        private string _path;
        private int _counter;
        private bool _started;
        private int _numberOfPictures;
        private double _durationBetweenPictures;
        private double _lastPictureTimeStamp;
        private Bitmap[] _bitmaps;
        private int _width;
        private int _height;
        private IPressedKeyDetector _keyDetector;
        private readonly IGameTimeProvider _gameTimeProvider;

        public ScreenshotMaker(string path, 
            int numberOfPictures,
            double durationBetweenPictures, 
            int width,
            int height, 
            IPressedKeyDetector keyDetector, 
            IGameTimeProvider gameTimeProvider)
        {
            _path = path;
            _numberOfPictures = numberOfPictures;
            _durationBetweenPictures = durationBetweenPictures;
            _bitmaps = new Bitmap[numberOfPictures];
            _width = width;
            _height = height;
            _keyDetector = keyDetector;
            _gameTimeProvider = gameTimeProvider;
        }

        private void DoScreenShot()
        {
            if (!_started)
                return;

            if (_counter >= _numberOfPictures)
                return;

            if (_lastPictureTimeStamp + _durationBetweenPictures > _gameTimeProvider.GetTotalTime())
                return;

            _lastPictureTimeStamp = _gameTimeProvider.GetTotalTime();

            _bitmaps[_counter] = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);

            System.Drawing.Graphics screenGraph = System.Drawing.Graphics.FromImage(_bitmaps[_counter]);

            screenGraph.CopyFromScreen(0, 0, 0, 0, new Size(_width, _height), CopyPixelOperation.SourceCopy);

            _counter++;

            if (_counter == _numberOfPictures)
            {
                for (int i = 0; i < _numberOfPictures; i++)
                {
                    _bitmaps[i].Save(_path + i + ".png", ImageFormat.Png);
                    _bitmaps[i].Dispose();
                }

                _started = false;
                _counter = 0;
            }
        }

        public void ExecuteLogic()
        {
            if (_started)
                DoScreenShot();
            else if (_keyDetector.IsKeyDown(Keys.F12))
                _started = true;
        }
    }
}
