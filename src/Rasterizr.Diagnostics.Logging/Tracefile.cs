using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Rasterizr.Core.Diagnostics;

namespace Rasterizr.Diagnostics.Logging
{
	public class Tracefile
	{
		public List<TracefileFrame> Frames { get; set; }

		public Tracefile()
		{
			Frames = new List<TracefileFrame>();
		}

		public static Tracefile FromFile(string filename)
		{
			return JsonConvert.DeserializeObject<Tracefile>(File.ReadAllText(filename));
		}

		public void Save(TextWriter textWriter)
		{
			textWriter.Write(JsonConvert.SerializeObject(this));
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