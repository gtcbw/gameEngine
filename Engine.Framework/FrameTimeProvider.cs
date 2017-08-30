using Engine.Contracts;
using System.Diagnostics;
using System.Threading;

namespace Engine.Framework
{
    public sealed class FrameTimeProvider : IFrameTimeProvider, IGameTimeProvider
    {
        private Stopwatch _stopwatch;
        private double _timeOfCurrentFrame;
        private double _totalTime;

        public FrameTimeProvider()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        double IFrameTimeProvider.GetTimeInSecondsSinceLastFrame()
        {
            return _timeOfCurrentFrame;
        }

        public void MeasureTimeSinceLastFrame()
        { 
            _stopwatch.Stop();
            decimal ticks = _stopwatch.ElapsedTicks;

            double timeInSeconds = (double)(ticks / Stopwatch.Frequency);

            if (timeInSeconds < 0.006)
            {
                int sleepTime = 6 - (int)(timeInSeconds * 1000);

                _stopwatch.Restart();
                Thread.Sleep(sleepTime);
                _stopwatch.Stop();
                ticks = _stopwatch.ElapsedTicks;
                timeInSeconds += (double)(ticks / Stopwatch.Frequency);
            }

            _stopwatch.Restart();

            _timeOfCurrentFrame = timeInSeconds;

            _totalTime += _timeOfCurrentFrame;
        }

        double IGameTimeProvider.GetTotalTime()
        {
            return _totalTime;
        }
    }
}
