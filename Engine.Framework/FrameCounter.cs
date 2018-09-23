using Engine.Contracts;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Framework
{
    public sealed class FrameCounter
    {
        private readonly IFontMapper _fontMapper;
        private readonly IFontRenderer _fontRenderer;
        private Stopwatch _stopwatch;
        private IEnumerable<int> _characters = new List<int>();
        private int _counter;

        public FrameCounter(IFontMapper fontMapper, 
            IFontRenderer fontRenderer)
        {
            _fontMapper = fontMapper;
            _fontRenderer = fontRenderer;
            _stopwatch = Stopwatch.StartNew();
        }

        public void MeasureAndRenderFramesPerSecond()
        {
            long milliSeconds = _stopwatch.ElapsedMilliseconds;

            if (milliSeconds >= 1000)
            {
                _characters = _fontMapper.ConvertTextToGameFont(_counter.ToString("d"));
                _counter = 0;
                _stopwatch.Restart();
            }

            _counter++;
            _fontRenderer.RenderFont(_characters, 0.8, 0.9);
        }
    }
}
