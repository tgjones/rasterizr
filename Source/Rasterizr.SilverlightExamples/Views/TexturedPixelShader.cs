using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.PixelShader;
using Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer.PipelineStages.ShaderStages.Core;
using Nexus;

namespace Apollo.Examples.SoftwareRasterizer.Views
{
	public struct TexturedPixelShaderInput
	{
		[Semantic(Semantics.TexCoord, 0)]
		public Point2D TexCoords;
	}

	public class TexturedPixelShader : PixelShaderBase<TexturedPixelShaderInput>
	{
		public SamplerState Sampler { get; set; }
		public Texture2D Texture { get; set; }

		public TexturedPixelShader()
		{
			Sampler = new SamplerState
			{
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				BorderColor = ColorsF.Blue,
				MagFilter = TextureFilter.Nearest
			};
		}

		public override Color Execute(TexturedPixelShaderInput pixelShaderInput)
		{
			return SampleTexture2D(Texture, Sampler, pixelShaderInput.TexCoords).ToRgbColor();
		}
	}
}