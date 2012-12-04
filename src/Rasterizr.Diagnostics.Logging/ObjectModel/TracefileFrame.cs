using System.Collections.Generic;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class TracefileFrame
	{
		public int Number { get; set; }
		public List<TracefileEvent> Events { get; set; }

		public TracefileFrame()
		{
			Events = new List<TracefileEvent>();
		}
	}
}