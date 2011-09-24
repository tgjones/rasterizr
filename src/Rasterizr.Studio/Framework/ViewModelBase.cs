using System.ComponentModel;

namespace Rasterizr.Studio.Framework
{
	/// <summary>
	/// Abstract base-class for a view-model.
	/// </summary>
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		/// <summary>
		/// Event raised when a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}