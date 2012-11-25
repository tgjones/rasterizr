using System;

namespace Rasterizr.Samples.Common
{
	public abstract class DemoApp
	{
		private readonly DemoTime clock = new DemoTime();
		private bool _disposed;
		private float _frameAccumulator;
		private int _frameCount;
		private DemoConfiguration _demoConfiguration;

		/// <summary>
		///   Performs object finalization.
		/// </summary>
		~DemoApp()
		{
			if (!_disposed)
			{
				Dispose(false);
				_disposed = true;
			}
		}

		/// <summary>
		///   Disposes of object resources.
		/// </summary>
		public void Dispose()
		{
			if (!_disposed)
			{
				Dispose(true);
				_disposed = true;
			}
			GC.SuppressFinalize(this);
		}

		/// <summary>
		///   Disposes of object resources.
		/// </summary>
		/// <param name = "disposeManagedResources">If true, managed resources should be
		///   disposed of in addition to unmanaged resources.</param>
		protected virtual void Dispose(bool disposeManagedResources)
		{
			
		}

		/// <summary>
		/// Gets the config.
		/// </summary>
		/// <value>The config.</value>
		public DemoConfiguration Config
		{
			get { return _demoConfiguration; }
		}

		/// <summary>
		///   Gets the number of seconds passed since the last frame.
		/// </summary>
		public float FrameDelta { get; private set; }

		/// <summary>
		///   Gets the number of seconds passed since the last frame.
		/// </summary>
		public float FramePerSecond { get; private set; }

		/// <summary>
		/// Runs the demo with default presentation
		/// </summary>
		public void Run()
		{
			Run(new DemoConfiguration());
		}

		/// <summary>
		/// Runs the demo.
		/// </summary>
		public void Run(DemoConfiguration demoConfiguration)
		{
			_demoConfiguration = demoConfiguration ?? new DemoConfiguration();
			CreateWindow();
			Initialize(_demoConfiguration);

			LoadContent();

			clock.Start();
			BeginRun();

			DoLoop();

			UnloadContent();
			EndRun();

			// Dispose explicity
			Dispose();
		}

		protected abstract void CreateWindow();
		protected abstract void DoLoop();

		/// <summary>
		///   In a derived class, implements logic to initialize the sample.
		/// </summary>
		protected abstract void Initialize(DemoConfiguration demoConfiguration);

		protected virtual void LoadContent()
		{
		}

		protected virtual void UnloadContent()
		{
		}

		/// <summary>
		///   In a derived class, implements logic to update any relevant sample state.
		/// </summary>
		protected virtual void Update(DemoTime time)
		{
		}

		/// <summary>
		///   In a derived class, implements logic to render the sample.
		/// </summary>
		protected virtual void Draw(DemoTime time)
		{
		}

		protected virtual void BeginRun()
		{
		}

		protected virtual void EndRun()
		{
		}

		/// <summary>
		///   In a derived class, implements logic that should occur before all
		///   other rendering.
		/// </summary>
		protected virtual void BeginDraw()
		{
		}

		/// <summary>
		///   In a derived class, implements logic that should occur after all
		///   other rendering.
		/// </summary>
		protected virtual void EndDraw()
		{
		}

		/// <summary>
		///   Quits the sample.
		/// </summary>
		public void Exit()
		{
			CloseWindow();
		}

		protected abstract void CloseWindow();

		/// <summary>
		///   Updates sample state.
		/// </summary>
		protected void OnUpdate()
		{
			FrameDelta = (float) clock.Update();
			Update(clock);
		}

		/// <summary>
		///   Renders the sample.
		/// </summary>
		protected void Render()
		{
			_frameAccumulator += FrameDelta;
			++_frameCount;
			if (_frameAccumulator >= 1.0f)
			{
				FramePerSecond = _frameCount / _frameAccumulator;

				SetWindowCaption(_demoConfiguration.Title + " - FPS: " + FramePerSecond);
				_frameAccumulator = 0.0f;
				_frameCount = 0;
			}

			BeginDraw();
			Draw(clock);
			EndDraw();
		}

		protected abstract void SetWindowCaption(string caption);
	}
}
