using System.IO;
using SlimShader.ObjectModel;

namespace SlimShader.Parser
{
	/// <summary>
	/// Most of this was adapted from 
	/// https://devel.nuclex.org/framework/browser/graphics/Nuclex.Graphics.Native/trunk/Source/Introspection/HlslShaderReflector.cpp?rev=1743
	/// </summary>
	public class ResourceDefinitionParser
	{
		private readonly DxbcReader _reader;

		public ResourceDefinitionParser(DxbcReader reader)
		{
			_reader = reader;
		}

		public ResourceDefinition Parse()
		{
			var index = _reader.CurrentIndex;

			uint constantBufferCount = _reader.ReadAndMoveNext();
			uint constantBufferOffset = _reader.ReadAndMoveNext();
			uint resourceBindingCount = _reader.ReadAndMoveNext();
			uint resourceBindingOffset = _reader.ReadAndMoveNext();
			uint requiredShaderVersion = _reader.ReadAndMoveNext();
			uint flags = _reader.ReadAndMoveNext();

			var result = new ResourceDefinition();

			_reader.Seek(index + constantBufferOffset, SeekOrigin.Begin);
			for (int i = 0; i < constantBufferOffset; i++)
			{
				result.ConstantBuffers.Add(ReadConstantBuffer());
			}

			return result;
		}

		private ConstantBuffer ReadConstantBuffer()
		{
			uint nameOffset = _reader.ReadAndMoveNext();

			return new ConstantBuffer
			{

			};
		}
	}
}