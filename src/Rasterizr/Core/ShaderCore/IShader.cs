using System;

namespace Rasterizr.Core.ShaderCore
{
	public interface IShader
	{
		Type InputType { get; }
		Type OutputType { get; }

		object BuildShaderInput();
		object Execute(object shaderInput);
	}
}