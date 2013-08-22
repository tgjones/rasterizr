namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
    internal class FpsCounter
    {
        private readonly SampleClock _clock;
        private float _lastElapsedTime;
        private float _frameAccumulator;
        private int _frameCount;

        public float FramesPerSecond { get; private set; }

        public FpsCounter(SampleClock clock)
        {
            _clock = clock;
        }

        public void Update()
        {
            var thisTime = _clock.TotalTime;
            var deltaTime = thisTime - _lastElapsedTime;
            _lastElapsedTime = thisTime;

            _frameAccumulator += deltaTime;
            ++_frameCount;
            if (_frameAccumulator >= 1.0f)
            {
                FramesPerSecond = _frameCount / _frameAccumulator;
                _frameAccumulator = 0.0f;
                _frameCount = 0;
            }
        }
    }
}