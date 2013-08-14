using System.Diagnostics;

namespace Rasterizr.SampleBrowser.Samples
{
	public class DemoTime
	{
		private readonly Stopwatch _stopwatch;
		private double _lastUpdate;

		public DemoTime()
		{
			_stopwatch = new Stopwatch();
		}

		public void Start()
		{
			_stopwatch.Start();
			_lastUpdate = 0;
		}

		public void Stop()
		{
			_stopwatch.Stop();
		}

		public double Update()
		{
			double now = ElapsedTime;
			double updateTime = now - _lastUpdate;
			_lastUpdate = now;
			return updateTime;
		}

		public float ElapsedTime
		{
			get { return _stopwatch.ElapsedMilliseconds * 0.001f; }
		}
	}
}