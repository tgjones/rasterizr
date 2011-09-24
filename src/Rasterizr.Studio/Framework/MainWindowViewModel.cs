using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Rasterizr.Studio.Documents.ModelDocument;
using Rasterizr.Studio.Documents.PresetSceneDocument;

namespace Rasterizr.Studio.Framework
{
	public class MainWindowViewModel : ViewModelBase
	{
		#region Fields

		private DocumentViewModelBase _activeDocument;
		private PaneViewModelBase _activePane;

		#endregion

		#region Properties

		/// <summary>
		/// View-models for documents.
		/// </summary>
		public ObservableCollection<DocumentViewModelBase> Documents
		{
			get;
			private set;
		}

		/// <summary>
		/// View-models for panes.
		/// </summary>
		public ObservableCollection<PaneViewModelBase> Panes
		{
			get;
			private set;
		}

		/// <summary>
		/// View-model for the active document.
		/// </summary>
		public DocumentViewModelBase ActiveDocument
		{
			get
			{
				return _activeDocument;
			}
			set
			{
				if (_activeDocument == value)
					return;

				_activeDocument = value;

				OnPropertyChanged("ActiveDocument");
				OnPropertyChanged("Title");

				if (ActiveDocumentChanged != null)
					ActiveDocumentChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Event raised when the ActiveDocument property has changed.
		/// </summary>
		public event EventHandler<EventArgs> ActiveDocumentChanged;

		/// <summary>
		/// View-model for the active pane.
		/// </summary>
		public PaneViewModelBase ActivePane
		{
			get
			{
				return _activePane;
			}
			set
			{
				if (_activePane == value)
					return;

				_activePane = value;

				OnPropertyChanged("ActivePane");
			}
		}

		#endregion

		#region Commands

		#region Open preset scene

		private RelayCommand _openPresetSceneCommand;

		public ICommand OpenPresetSceneCommand
		{
			get
			{
				return _openPresetSceneCommand ?? (_openPresetSceneCommand = new RelayCommand(x =>
				{
					Type type = Type.GetType(x.ToString());
					Documents.Add(new PresetSceneDocumentViewModel(type));
				}));
			}
		}

		#endregion

		#region Open

		private RelayCommand _openCommand;
		public ICommand OpenCommand
		{
			get
			{
				return _openCommand ?? (_openCommand = new RelayCommand(x =>
				{
					// Configure open file dialog box
					var dialog = new OpenFileDialog
					{
						DefaultExt = ".obj",
						Filter = "3D models|*.obj;*.3ds;*.nff;*.ply"
					};

					// Show open file dialog box
					if (dialog.ShowDialog() == true)
						Documents.Add(new ModelDocumentViewModel(dialog.FileName));
				}));
			}
		}

		#endregion 

		#region Exit

		private RelayCommand _exitCommand;
		public ICommand ExitCommand
		{
			get { return _exitCommand ?? (_exitCommand = new RelayCommand(x => Application.Current.Shutdown())); }
		}

		#endregion 

		#endregion

		public MainWindowViewModel()
		{
			Documents = new ObservableCollection<DocumentViewModelBase>();
			Panes = new ObservableCollection<PaneViewModelBase>();
		}
	}
}