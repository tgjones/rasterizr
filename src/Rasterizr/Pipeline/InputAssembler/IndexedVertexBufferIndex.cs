namespace Rasterizr.Pipeline.InputAssembler
{
	internal class IndexedVertexBufferIndex : VertexBufferIndex
	{
		private readonly IndexBufferBinding _indexBufferBinding;
		private readonly int _offset;

		public IndexedVertexBufferIndex(
			IndexBufferBinding indexBufferBinding, int startIndexLocation,
			VertexBufferBinding vertexBufferBinding, int startVertexLocation)
			: base(vertexBufferBinding, startVertexLocation)
		{
			_indexBufferBinding = indexBufferBinding;
			_offset = indexBufferBinding.Offset + (startIndexLocation * FormatHelper.SizeOfInBytes(indexBufferBinding.Format));
		}

		protected override int GetIndex(int index)
		{
			if (_indexBufferBinding.Format == Format.R32_UInt)
			{
				var data = new uint[1];
				_indexBufferBinding.Buffer.GetData(data, _offset + (index * 4), 4);
				return (int)data[0];
			}
			else
			{
				var data = new ushort[1];
				_indexBufferBinding.Buffer.GetData(data, _offset + (index * 2), 2);
				return data[0];
			}
		}
	}
}