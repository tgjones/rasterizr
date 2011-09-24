namespace Rasterizr.Studio.Framework
{
	/// <summary>
	/// Abstract base class for an AvalonDock pane view-model.
	/// </summary>
	public abstract class PaneViewModelBase : ViewModelBase
	{
		/// <summary>
		/// Set to 'true' when the pane is visible.
		/// </summary>
		private bool _isVisible = true;
		public bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			set
			{
				if (_isVisible == value)
					return;
				_isVisible = value;
				OnPropertyChanged("IsVisible");
			}
		}
	}
}