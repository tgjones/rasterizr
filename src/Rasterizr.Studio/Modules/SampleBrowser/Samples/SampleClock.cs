using System.Diagnostics;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
	internal class SampleClock
	{
		private readonly Stopwatch _stopwatch;
	    
        internal float ManuallySteppedTime { get; set; }

	    public float TotalTime
	    {
            get { return ManuallySteppedTime + _stopwatch.ElapsedMilliseconds * 0.001f; }
	    }

	    public SampleClock()
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
	}
}