using System;

namespace Rasterizr.ShaderStages
{
	public abstract class ShaderBase
	{
		// TODO: Intrinsic functions available to all shaders.
	}

	public abstract class ShaderBase<TShaderInput, TShaderOutput> : ShaderBase, IShader
	{
		public Type InputType
		{
			get { return typeof(TShaderInput); }
		}

		public Type OutputType
		{
			get { return typeof(TShaderOutput); }
		}

		public abstract TShaderOutput Execute(TShaderInput vertexShaderInput);

		public object Execute(object vertexShaderInput)
		{
			// Cast shader input to strongly-typed TShaderInput.
			var input = (TShaderInput)vertexShaderInput;

			// Execute VertexShader.
			TShaderOutput output = Execute(input);

			// Convert TVertexShaderOutput to VertexShaderOutput.
			return output;
		}
	}
}