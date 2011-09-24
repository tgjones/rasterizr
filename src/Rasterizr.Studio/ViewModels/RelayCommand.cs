using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using System.Windows.Threading;

namespace Rasterizr.Demo.ViewModels
{
	/// <summary>
	/// Represents base command
	/// </summary>
	public class RelayCommand : ICommand
	{
		#region CanExecute Automatic Updating

		[SuppressMessage("Microsoft.Performance", "CA1823", Justification = "This variable is used. I can swear")]
		static DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromMilliseconds(200), DispatcherPriority.SystemIdle, (s, e) => UpdateCanExcecute(), Dispatcher.CurrentDispatcher);

		static void UpdateCanExcecute()
		{
			foreach (var command in automaticCanExecuteUpdatingCommand)
			{
				command.RaiseCanExecuteChanged();
			}
		}

		static List<RelayCommand> automaticCanExecuteUpdatingCommand = new List<RelayCommand>();

		static void RegisterForCanExecuteUpdating(RelayCommand command)
		{
			automaticCanExecuteUpdatingCommand.Add(command);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when CanExecute property changed
		/// </summary>
		public event EventHandler CanExecuteChanged;

		#endregion

		#region Fields

		readonly Action<object> execute;
		readonly Predicate<object> canExecute;

		#endregion // Fields

		#region Constructors

		/// <summary>
		/// Construcotor
		/// </summary>
		/// <param name="execute">Action to execute</param>
		public RelayCommand(Action<object> execute)
			: this(execute, null, false)
		{
		}

		/// <summary>
		/// Construcotor
		/// </summary>
		/// <param name="execute">Action to execute</param>
		/// <param name="canExecute">Predicate to check whether command can be executed</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
			: this(execute, canExecute, false)
		{

		}

		/// <summary>
		/// Construcotor
		/// </summary>
		/// <param name="execute">Action to execute</param>
		/// <param name="canExecute">Predicate to check whether command can be executed</param>
		/// <param name="autoCanExecuteUpdating">Use this flag only if you can not invoke RaiseCanExecuteChanged</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute, bool autoCanExecuteUpdating)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			this.execute = execute;
			this.canExecute = canExecute;

			if (autoCanExecuteUpdating) RegisterForCanExecuteUpdating(this);
		}

		#endregion

		#region ICommand Members

		/// <summary>
		/// Gets whether the command can be executed
		/// </summary>
		/// <param name="parameter">Parameter</param>
		/// <returns></returns>
		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return canExecute == null ? true : canExecute(parameter);
		}

		/// <summary>
		/// Raises CanExecuteChanged event 
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1030")]
		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null) CanExecuteChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Executes the command
		/// </summary>
		/// <param name="parameter"></param>
		public void Execute(object parameter)
		{
			execute(parameter);
		}

		#endregion
	}
}