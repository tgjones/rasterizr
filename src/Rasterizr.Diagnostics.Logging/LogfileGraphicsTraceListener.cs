using System.IO;
using Newtonsoft.Json;
using Rasterizr.Core.Diagnostics;

namespace Rasterizr.Diagnostics.Logging
{
	public class LogfileGraphicsTraceListener : GraphicsTraceListener
	{
		private readonly JsonTextWriter _writer;

		public LogfileGraphicsTraceListener(string fileName)
		{
			_writer = new JsonTextWriter(new StreamWriter(fileName));
			_writer.WriteStartArray();
		}

		public override void BeginApiCall(string methodName, params object[] methodArguments)
		{
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

		public override void EndApiCall()
		{
			
		}

		public override void Close()
		{
			_writer.WriteEndArray();
			_writer.Close();
			
		}
	}
}
