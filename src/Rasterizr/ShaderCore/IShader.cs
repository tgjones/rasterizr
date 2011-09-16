using System;

namespace Rasterizr.ShaderCore
{
	public interface IShader
	{
		Type InputType { get; }
		Type OutputType { get; }

		object BuildShaderInput();
		object Execute(object shaderInput);
	}
}