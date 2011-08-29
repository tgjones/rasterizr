using System.Collections.Generic;
using Nexus;
using Rasterizr.ShaderStages.Core;

namespace Rasterizr
{
	public class ModelMesh
	{
		private readonly RasterizrDevice _device;

		public List<VertexPositionNormalTexture> Vertices { get; set; }
		public Int32Collection Indices { get; set; }

		public Effect Effect { get; set; }

		public ModelMesh(RasterizrDevice device)
		{
			_device = device;
		}

		public void Draw()
		{
			_device.RenderPipeline.InputAssembler.InputLayout = VertexPositionNormalTexture.InputLayout;
			_device.RenderPipeline.InputAssembler.Vertices = Vertices;
			_device.RenderPipeline.InputAssembler.Indices = Indices;

			foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_device.Draw();
			}
		}
	}
}