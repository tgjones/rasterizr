using System.Collections.Generic;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class TracefileEvent
	{
		public int Number { get; set; }
		public OperationType OperationType { get; set; }
		public TracefileEventArgumentCollection Arguments { get; set; }

        internal List<InputAssemblerVertexOutput> InputAssemblerOutputs { get; set; }
		internal List<PixelEvent> PixelEvents { get; set; }

		public TracefileEvent()
		{
			Arguments = new TracefileEventArgumentCollection();
            InputAssemblerOutputs = new List<InputAssemblerVertexOutput>();
			PixelEvents = new List<PixelEvent>();
		}
	}
}