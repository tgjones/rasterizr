using System;

namespace Rasterizr.ShaderStages
{
	public interface IShader
	{
		Type InputType { get; }
		Type OutputType { get; }

		object BuildShaderInput();
		object Execute(object shaderInput);
	}
}