using System.IO;
using System.Linq;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline.InputAssembler;

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

			var arguments = (methodArguments != null)
				? methodArguments.Select(ConvertArgument).ToList()
				: null;

			_currentFrame.Events.Add(new TracefileEvent
			{
				Number = ++_operationNumber,
				OperationType = type,
				Arguments = new TracefileEventArgumentCollection(arguments)
			});

			if (type == OperationType.SwapChainPresent)
			{
				_currentFrame = null;
				_writtenBeginFrame = false;
				_operationNumber = 0;
			}
		}

		private object ConvertArgument(object arg)
		{
			if (arg is VertexBufferBinding[])
				return ((VertexBufferBinding[]) arg).Select(x => new SerializedVertexBufferBinding
				{
					Buffer = x.Buffer.ID,
					Offset = x.Offset,
					Stride = x.Stride
				});
			return arg;
		}

		public void Flush()
		{
			_tracefile.Save(_textWriter);
		}
	}
}