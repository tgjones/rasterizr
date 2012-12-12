using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class TracefileEvent
	{
		public int Number { get; set; }
		public OperationType OperationType { get; set; }
		public TracefileEventArgumentCollection Arguments { get; set; }

		[JsonIgnore]
		public List<PixelHistoryEvent> PixelHistoryEvents { get; set; }

		public TracefileEvent()
		{
			Arguments = new TracefileEventArgumentCollection();
			PixelHistoryEvents = new List<PixelHistoryEvent>();
		}
	}
}