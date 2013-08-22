namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
    internal class FpsCounter
    {
        private readonly DemoTime _clock;

        private float _frameAccumulator;
        private int _frameCount;

        public float FramesPerSecond { get; private set; }

        public FpsCounter(DemoTime clock)
        {
            _clock = clock;
        }

        public void Update()
        {
            var frameDelta = _clock.DeltaTime;

            _frameAccumulator += frameDelta;
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