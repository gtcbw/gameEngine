using Engine.Contracts;

namespace Engine.Framework
{
    public sealed class PercentProvider : IPercentProvider
    {
        private readonly IFrameTimeProvider _frameTimeProvider;
        private readonly double _duration;

        private double _currentDuration;

        public PercentProvider(IFrameTimeProvider frameTimeProvider, double duration)
        {
            _frameTimeProvider = frameTimeProvider;
            _currentDuration = _duration = duration;
        }

        double IPercentProvider.GetPercent()
        {
            _currentDuration += _frameTimeProvider.GetTimeInSecondsSinceLastFrame();

            double percent = _currentDuration / _duration;

            return percent <= 1.0 ? percent : 1.0;
        }

        bool IPercentProvider.IsOver()
        {
            return _currentDuration >= _duration;
        }

        void IPercentProvider.Start()
        {
            _currentDuration = 0.0;
        }
    }
}
