using Nexus;
using Nexus.Graphics.Colors;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Toolkit;

namespace Rasterizr.Studio.Modules.GraphicsDebugging.Design
{
	public class DesignTimeTracefile : Tracefile
	{
		public DesignTimeTracefile()
		{
			Frames.Add(
				new TracefileFrame
				{
					Number = 1,
					Events =
					{
						new TracefileEvent
						{
							Number = 1,
							OperationType = OperationType.InputAssemblerSetVertices,
							Arguments =
							{
								new[]
								{
									new VertexPositionColor(new Point3D(1, 2, 3), ColorsF.Red),
									new VertexPositionColor(new Point3D(4, 5, 6), ColorsF.Green)
								}
							}
						}
					}
				});
		}
	}
}