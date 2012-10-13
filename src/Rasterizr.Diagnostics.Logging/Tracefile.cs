using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Rasterizr.Core.Diagnostics;

namespace Rasterizr.Diagnostics.Logging
{
	public class Tracefile
	{
		private static readonly JsonSerializerSettings SerializerSettings;

		static Tracefile()
		{
			SerializerSettings = new JsonSerializerSettings
			{
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				Formatting = Formatting.Indented,
				TypeNameHandling = TypeNameHandling.Auto
			};
		}

		public List<TracefileFrame> Frames { get; set; }

		public Tracefile()
		{
			Frames = new List<TracefileFrame>();
		}

		public static Tracefile FromTextReader(TextReader textReader)
		{
			return JsonConvert.DeserializeObject<Tracefile>(textReader.ReadToEnd(), SerializerSettings);
		}

		public void Save(TextWriter textWriter)
		{
			textWriter.Write(JsonConvert.SerializeObject(this, SerializerSettings));
		}
	}

	public class TracefileFrame
	{
		public int Number { get; set; }
		public List<TracefileEvent> Events { get; set; }

		public TracefileFrame()
		{
			Events = new List<TracefileEvent>();
		}
	}

	public class TracefileEvent
	{
		public int Number { get; set; }
		public OperationType OperationType { get; set; }
		public List<object> Arguments { get; set; }

		public TracefileEvent()
		{
			Arguments = new List<object>();
		}
	}
}