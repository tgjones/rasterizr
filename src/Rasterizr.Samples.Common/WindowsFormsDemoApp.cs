using System;
using System.Windows.Forms;
using Rasterizr.Platform.WindowsForms;

namespace Rasterizr.Samples.Common
{
	public abstract class WindowsFormsDemoApp : DemoApp
	{
		private FormWindowState _currentFormWindowState;
		private Form _form;

		protected override void Dispose(bool disposeManagedResources)
		{
			if (disposeManagedResources)
			{
				if (_form != null)
					_form.Dispose();
			}

			base.Dispose(disposeManagedResources);
		}

		protected Form Form
		{
			get { return _form; }
		}

		/// <summary>
		/// Return the Handle to display to.
		/// </summary>
		protected IntPtr DisplayHandle
		{
			get { return _form.Handle; }
		}

		/// <summary>
		/// Create Form for this demo.
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		protected virtual Form CreateForm(DemoConfiguration config)
		{
			return new RenderForm(config.Title)
			{
				ClientSize = new System.Drawing.Size(config.Width, config.Height)
			};
		}

		protected override void CreateWindow()
		{
			_form = CreateForm(Config);
		}

		protected override void DoLoop()
		{
			bool isFormClosed = false;
			bool formIsResizing = false;

			_form.MouseClick += HandleMouseClick;
			_form.KeyDown += HandleKeyDown;
			_form.KeyUp += HandleKeyUp;
			_form.Resize += (o, args) =>
			{
				if (_form.WindowState != _currentFormWindowState)
				{
					HandleResize(o, args);
				}

				_currentFormWindowState = _form.WindowState;
			};

			_form.ResizeBegin += (o, args) => { formIsResizing = true; };
			_form.ResizeEnd += (o, args) =>
			{
				formIsResizing = false;
				HandleResize(o, args);
			};

			_form.Closed += (o, args) => { isFormClosed = true; };

			RenderLoop.Run(_form, () =>
			{
				if (isFormClosed)
				{
					return;
				}

				OnUpdate();
				if (!formIsResizing)
					Render();
			});
		}

		protected virtual void MouseClick(MouseEventArgs e)
		{
		}

		protected virtual void KeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Exit();
		}

		protected virtual void KeyUp(KeyEventArgs e)
		{
		}

		/// <summary>
		///   Handles a mouse click event.
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
		private void HandleMouseClick(object sender, MouseEventArgs e)
		{
			MouseClick(e);
		}

		/// <summary>
		///   Handles a key down event.
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
		private void HandleKeyDown(object sender, KeyEventArgs e)
		{
			KeyDown(e);
		}

		/// <summary>
		///   Handles a key up event.
		/// </summary>
		/// <param name = "sender">The sender.</param>
		/// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
		private void HandleKeyUp(object sender, KeyEventArgs e)
		{
			KeyUp(e);
		}

		private void HandleResize(object sender, EventArgs e)
		{
			if (_form.WindowState == FormWindowState.Minimized)
			{
				return;
			}
		}

		protected System.Drawing.Size RenderingSize
		{
			get
			{
				return _form.ClientSize;
			}
		}

		protected override void SetWindowCaption(string caption)
		{
			_form.Text = caption;
		}

		protected override void CloseWindow()
		{
			_form.Close();
		}
	}
}