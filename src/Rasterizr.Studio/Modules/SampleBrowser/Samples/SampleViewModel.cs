using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Micro;
using Gemini.Framework.Services;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.TracefileViewer.ViewModels;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
	public class SampleViewModel : Screen, ISample
	{
	    private const float TimeStep = (1000.0f / 60) / 1000.0f;

	    private readonly SampleBase _sample;
		private readonly SampleClock _clock;
	    private readonly FpsCounter _fpsCounter;
		private float _totalTime;
		private readonly DispatcherTimer _timer;
		private float _framePerSecond;
		private Image _image;

	    private bool _isPaused;
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                NotifyOfPropertyChange(() => IsPaused);
            }
        }

	    private string _playPauseText = "Pause";
	    public string PlayPauseText
	    {
	        get { return _playPauseText; }
	        set
	        {
	            _playPauseText = value;
                NotifyOfPropertyChange(() => PlayPauseText);
	        }
	    }

		public SampleViewModel(SampleBase sample)
		{
		    _sample = sample;
		    _clock = new SampleClock();
		    _fpsCounter = new FpsCounter(_clock);

		    DisplayName = sample.Name;

			_timer = new DispatcherTimer();
			_timer.Tick += (sender, e) =>
			{
			    if (_isPaused)
			        return;

			    var realTime = _clock.TotalTime;
			    while (_totalTime < realTime)
			        _totalTime += TimeStep;
                
                _fpsCounter.Update();
                FramePerSecond = _fpsCounter.FramesPerSecond;

                Render();
			};
		}

	    public void TogglePlayPause()
	    {
	        if (IsPaused)
	        {
	            PlayPauseText = "Pause";
	            IsPaused = false;
                _clock.Start();
            }
	        else
	        {
	            PlayPauseText = "Play";
	            IsPaused = true;
	            _clock.Stop();
	        }
	    }

        public void StepBackward()
        {
            _clock.ManuallySteppedTime -= TimeStep;
            _totalTime -= TimeStep;
            Render();
        }

	    public void StepForward()
	    {
            _clock.ManuallySteppedTime += TimeStep;
	        _totalTime += TimeStep;
	        Render();
	    }

        public void AnalyzeLastFrame()
	    {
	        // Create new instance of sample object.
	        var newSample = (SampleBase) Activator.CreateInstance(_sample.GetType());
            IoC.BuildUp(newSample);

            // Create logger.
            var device = new Device();
            var logger = new TracefileBuilder(device);

            // Initialize and draw.
            newSample.Initialize(device);
            newSample.Draw(_totalTime);

            var tracefileDocument = new TracefileViewerViewModel(
                IoC.Get<ISelectionService>(), 
                "[Sample " + _sample.Name + "]",
                logger.Tracefile);
            IoC.Get<IShell>().OpenDocument(tracefileDocument);
	    }

		protected override void OnViewLoaded(object view)
		{
			_image = ((SampleView) view).Image;
			Activate();

			base.OnViewLoaded(view);
		}

		private void Activate()
		{
			_clock.Start();

			_image.Source = _sample.Initialize(new Device());

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
			_clock.Stop();

			base.OnDeactivate(close);
		}

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
		/// Renders the sample.
		/// </summary>
		private void Render()
		{
			_sample.Draw(_totalTime);
		}
	}
}