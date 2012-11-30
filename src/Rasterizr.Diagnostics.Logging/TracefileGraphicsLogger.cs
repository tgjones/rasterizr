using System.IO;
using System.Linq;

namespace Rasterizr.Diagnostics.Logging
{
	public class TracefileGraphicsLogger : GraphicsLogger
	{
		private readonly TextWriter _textWriter;
		private readonly Tracefile _tracefile;
		private TracefileFrame _currentFrame;
		private bool _writtenBeginFrame;
		private int _frameNumber;
		private int _operationNumber;

		public TracefileGraphicsLogger(TextWriter textWriter)
		{
			_textWriter = textWriter;
			_tracefile = new Tracefile();
		}

		protected internal override void BeginOperation(OperationType type, params object[] methodArguments)
		{
			if (!_writtenBeginFrame)
			{
				_tracefile.Frames.Add(_currentFrame = new TracefileFrame
				{
					Number = ++_frameNumber
				});
				_writtenBeginFrame = true;
			}

			var arguments = (methodArguments != null)
				? methodArguments.Select(x =>
				{
					if (x is DeviceChild)
						return ((DeviceChild) x).ID;
					return x;
				}).ToList()
				: null;

			_currentFrame.Events.Add(new TracefileEvent
			{
				Number = ++_operationNumber,
				OperationType = type,
				Arguments = arguments
			});

			if (type == OperationType.SwapChainPresent)
			{
				_currentFrame = null;
				_writtenBeginFrame = false;
				_operationNumber = 0;
			}
		}

		public void Close()
		{
			_tracefile.Save(_textWriter);
			_textWriter.Dispose();
		}
	}
}