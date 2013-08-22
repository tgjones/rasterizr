using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline.InputAssembler;
using SlimShader.Chunks.Xsgn;

namespace Rasterizr.Diagnostics.Logging
{
	public class TracefileGraphicsLogger : GraphicsLogger
	{
		private readonly bool _includePixelEvents;
		private readonly Tracefile _tracefile;
		private TracefileFrame _currentFrame;
		private bool _writtenBeginFrame;
		private int _frameNumber;
		private int _operationNumber;

        public TracefileGraphicsLogger(bool includePixelEvents)
        {
            _includePixelEvents = includePixelEvents;
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
            if (arg is InputOutputSignatureChunk)
                return ((InputOutputSignatureChunk) arg).Container.RawBytes;
			return arg;
		}

		protected override void AddPixelEvent(PixelEvent @event)
		{
			if (!_includePixelEvents)
				return;
			var lastEvent = _currentFrame.Events.Last();
			lastEvent.PixelEvents.Add(@event);
		}

		public IEnumerable<TracefileEvent> GetEvents(int frame, int x, int y)
		{
			return _tracefile.Frames.Single(f => f.Number == frame).Events
				.Where(e => e.PixelEvents.Any(phe => phe.Matches(x, y)))
				.Select(e => new TracefileEvent
				{
					Arguments = e.Arguments,
					Number = e.Number,
					OperationType = e.OperationType,
					PixelEvents = e.PixelEvents.Where(phe => phe.Matches(x, y)).ToList()
				});
		}

		public void WriteTo(TextWriter textWriter)
		{
            _tracefile.Save(textWriter);
		}
	}
}