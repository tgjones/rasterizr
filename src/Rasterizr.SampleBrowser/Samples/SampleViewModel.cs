using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Micro;

namespace Rasterizr.SampleBrowser.Samples
{
	public class SampleViewModel : Screen
	{
		private readonly SampleBase _sample;
		private readonly DemoTime _clock = new DemoTime();
		private float _frameAccumulator;
		private int _frameCount;
		private readonly DispatcherTimer _timer;
		private float _framePerSecond;
		private Image _image;

		public override string DisplayName
		{
			get { return _sample.Name; }
			set { base.DisplayName = value; }
		}

		public SampleViewModel(SampleBase sample)
		{
			_sample = sample;

			_timer = new DispatcherTimer();
			_timer.Tick += (sender, e) =>
			{
				OnUpdate();
				Render();
			};
		}

		protected override void OnViewLoaded(object view)
		{
			_image = ((SampleView)view).Image;
			Activate();

			base.OnViewLoaded(view);
		}

		private void Activate()
		{
			_clock.Start();

			_sample.Initialize(_image);
			_sample.BeginRun();

			_timer.Start();
		}

		protected override void OnActivate()
		{
			if (_image == null)
				return;

			Activate();

			base.OnActivate();
		}

		protected override void OnDeactivate(bool close)
		{
			_timer.Stop();

			_sample.EndRun();

			_clock.Stop();

			base.OnDeactivate(close);
		}

		/// <summary>
		/// Gets the number of seconds passed since the last frame.
		/// </summary>
		public float FrameDelta { get; private set; }

		/// <summary>
		/// Gets the number of frames per second.
		/// </summary>
		public float FramePerSecond
		{
			get { return _framePerSecond; }
			private set
			{
				_framePerSecond = value;
				NotifyOfPropertyChange(() => FramePerSecond);
			}
		}

		/// <summary>
		///   Updates sample state.
		/// </summary>
		private void OnUpdate()
		{
			FrameDelta = (float) _clock.Update();
			_sample.Update(_clock);
		}

		/// <summary>
		///   Renders the sample.
		/// </summary>
		private void Render()
		{
			_frameAccumulator += FrameDelta;
			++_frameCount;
			if (_frameAccumulator >= 1.0f)
			{
				FramePerSecond = _frameCount / _frameAccumulator;
				_frameAccumulator = 0.0f;
				_frameCount = 0;
			}

			_sample.BeginDraw();
			_sample.Draw(_clock);
			_sample.EndDraw();
		}
	}
}