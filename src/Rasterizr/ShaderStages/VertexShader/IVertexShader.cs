using System;
namespace Rasterizr.ShaderStages.VertexShader
{
	public interface IVertexShader
	{
		Type VertexShaderInputType { get; }
		Type VertexShaderOutputType { get; }

		IVertexShaderOutput Execute(object vertexShaderInput);
	}
}