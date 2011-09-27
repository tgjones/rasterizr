using Nexus;

namespace Rasterizr.Core.ShaderCore
{
	public class TransformedVertex
	{
		public object ShaderOutput { get; private set; }
		public Point4D Position { get; set; }

		public TransformedVertex(object shaderOutput, Point4D position)
		{
			ShaderOutput = shaderOutput;
			Position = position;
		}
	}
}