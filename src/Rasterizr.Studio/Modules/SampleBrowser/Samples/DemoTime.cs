using System.Diagnostics;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
	internal class DemoTime
	{
		private readonly Stopwatch _stopwatch;
	    private float _lastElapsedTime;

	    public float DeltaTime { get; private set; }

	    public DemoTime()
		{
			_stopwatch = new Stopwatch();
		}

		public void Start()
		{
			_stopwatch.Start();
		}

		public void Stop()
		{
			_stopwatch.Stop();
		}

	    public void Update()
	    {
	        var thisTime = _stopwatch.ElapsedMilliseconds * 0.001f;
            DeltaTime = thisTime - _lastElapsedTime;
	        _lastElapsedTime = thisTime;
	    }
	}
}