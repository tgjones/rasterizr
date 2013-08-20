using System.Windows.Controls;
using Caliburn.Micro;
using Rasterizr.SampleBrowser.Framework;
using Rasterizr.SampleBrowser.Samples;

namespace Rasterizr.SampleBrowser.FrameAnalysis
{
	public class FrameAnalyzerViewModel : Screen, ISample
	{
		private readonly SampleBase _sample;
		private Image _image;

		public override string DisplayName
		{
			get { return _sample.Name; }
			set { base.DisplayName = value; }
		}

        public FrameAnalyzerViewModel(SampleBase sample)
		{
			_sample = sample;
		}

		protected override void OnViewLoaded(object view)
		{
			_image = ((FrameAnalyzerView) view).Image;
			Activate();

			base.OnViewLoaded(view);
		}

		private void Activate()
		{
			_sample.Initialize(_image);
			_sample.BeginRun();

		    const float time = 2;

            _sample.Update(time);

            _sample.BeginDraw();
            _sample.Draw(time);
            _sample.EndDraw();

            _sample.EndRun();
		}

		protected override void OnActivate()
		{
			if (_image == null)
				return;

			Activate();

			base.OnActivate();
		}
	}
}