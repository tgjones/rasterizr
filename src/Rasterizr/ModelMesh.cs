using System.Collections.Generic;
using Nexus;
using Rasterizr.Core;
using Rasterizr.Core.InputAssembler;
using Rasterizr.Effects;

namespace Rasterizr
{
	public class ModelMesh
	{
		private readonly RasterizrDevice _device;
		private InputLayout _inputLayout;
		private Effect _effect;

		public List<VertexPositionNormalTexture> Vertices { get; set; }
		public Int32Collection Indices { get; set; }

		public Effect Effect
		{
			get { return _effect; }
			set
			{
				_effect = value;
				_inputLayout = new InputLayout(VertexPositionNormalTexture.InputElements,
					Effect.CurrentTechnique.Passes[0].VertexShader);
			}
		}

		public string Name { get; set; }

		public ModelMesh(RasterizrDevice device)
		{
			_device = device;
		}

		public void Draw()
		{
			_device.InputAssembler.InputLayout = _inputLayout;
			_device.InputAssembler.Vertices = Vertices;
			_device.InputAssembler.Indices = Indices;

			foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				_device.DrawIndexed(Indices.Count, 0, 0);
			}
		}
	}
}