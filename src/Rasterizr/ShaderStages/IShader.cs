using System;

namespace Rasterizr.ShaderStages
{
	public interface IShader
	{
		Type InputType { get; }
		Type OutputType { get; }

		object Execute(object shaderInput);
	}
}