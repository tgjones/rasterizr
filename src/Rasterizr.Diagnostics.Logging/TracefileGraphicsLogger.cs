using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Rasterizr.Core.Diagnostics;

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

		protected override void BeginOperation(OperationType type, params object[] methodArguments)
		{
			if (!_writtenBeginFrame)
			{
				_tracefile.Frames.Add(_currentFrame = new TracefileFrame
				{
					Number = ++_frameNumber
				});
				_writtenBeginFrame = true;
			}

			_currentFrame.Events.Add(new TracefileEvent
			{
				Number = ++_operationNumber,
				OperationType = type,
				Arguments = (methodArguments == null) ? null : methodArguments.ToList()
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
			_textWriter.Close();
		}
	}
}
