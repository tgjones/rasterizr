using System;

namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.ShaderStages.Core
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SemanticAttribute : Attribute
	{
		public string Name { get; set; }
		public int Index { get; set; }

		public SemanticAttribute(string name, int index = 0)
		{
			Name = name;
			Index = index;
		}
	}
}