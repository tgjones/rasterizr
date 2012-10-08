using System.IO;
using Newtonsoft.Json;
using Rasterizr.Core.Diagnostics;

namespace Rasterizr.Diagnostics.Logging
{
	public class LogfileGraphicsLogger : GraphicsLogger
	{
		private readonly JsonTextWriter _writer;
		private bool _writtenBeginFrame;
		private int _frameNumber;

		public LogfileGraphicsLogger(TextWriter textWriter)
		{
			_writer = new JsonTextWriter(textWriter);
			_writer.WriteStartArray();
		}

		protected override void BeginApiCall(string methodName, params object[] methodArguments)
		{
			if (!_writtenBeginFrame)
			{
				_writer.WriteStartObject();
				_writer.WritePropertyName("frame");
				_writer.WriteValue(_frameNumber++);
				_writer.WritePropertyName("calls");
				_writer.WriteStartArray();
				_writtenBeginFrame = true;
			}

			_writer.WriteStartObject();

			_writer.WritePropertyName("method");
			_writer.WriteValue(methodName);

			_writer.WritePropertyName("arguments");
			_writer.WriteStartArray();
			if (methodArguments != null)
				foreach (var argument in methodArguments)
					_writer.WriteValue(argument);
			_writer.WriteEndArray();

			_writer.WriteEndObject();
		}

		protected override void EndFrame()
		{
			_writer.WriteEndArray();
			_writer.WriteEndObject();
			_writtenBeginFrame = false;
		}
	}
}
