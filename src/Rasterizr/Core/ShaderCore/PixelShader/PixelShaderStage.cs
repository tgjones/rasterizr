using System.Collections.Generic;
using Rasterizr.Core.Rasterizer;

namespace Rasterizr.Core.ShaderCore.PixelShader
{
	public class PixelShaderStage : PipelineStageBase<FragmentQuad, Pixel>
	{
		private readonly PixelShaderThreadedExecutor _threadedExecutor;
		private IPixelShader _pixelShader;
		private ShaderDescription _pixelShaderDescription;

		public IPixelShader PixelShader
		{
			get { return _pixelShader; }
			set
			{
				_pixelShader = value;
				_pixelShaderDescription = ShaderDescriptionCache.GetDescription(_pixelShader);
			}
		}

		public PixelShaderStage()
		{
			_threadedExecutor = new PixelShaderThreadedExecutor();
		}

		public override IEnumerable<Pixel> Run(IEnumerable<FragmentQuad> inputs)
		{
			var textures = _pixelShaderDescription.GetTextureParameters(_pixelShader);
			foreach (var texture in textures)
				texture.BeginPixelShader(PixelShader);

			// Process groups of four fragments together.
			foreach (var fragmentQuad in inputs)
			{
				var pixels = _threadedExecutor.Execute(PixelShader, fragmentQuad);
				foreach (var pixel in pixels)
					yield return pixel;
			}

			foreach (var texture in textures)
				texture.EndPixelShader();
		}

		public object BuildPixelShaderInput()
		{
			return PixelShader.BuildShaderInput();
		}
	}
}