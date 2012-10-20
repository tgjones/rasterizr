using System.IO;

namespace SlimShader.IO
{
	public class BytecodeReader
	{
		private readonly byte[] _buffer;
		private readonly int _offset;
		private readonly BinaryReader _reader;

		public bool EndOfBuffer
		{
			get { return _reader.BaseStream.Position >= _reader.BaseStream.Length; }
		}

		public long CurrentPosition
		{
			get { return _reader.BaseStream.Position; }
		}

		public BytecodeReader(byte[] buffer, int index, int count)
		{
			_buffer = buffer;
			_offset = index;
			_reader = new BinaryReader(new MemoryStream(buffer, index, count));
		}

		public float ReadSingle()
		{
			return _reader.ReadSingle();
		}

		public uint ReadUInt32()
		{
			return _reader.ReadUInt32();
		}

		public ulong ReadUInt64()
		{
			return _reader.ReadUInt64();
		}

		public BytecodeReader CopyAtCurrentPosition(int? count = null)
		{
			return CopyAtOffset((int) _reader.BaseStream.Position, count);
		}

		public BytecodeReader CopyAtOffset(int offset, int? count = null)
		{
			count = count ?? (int) (_reader.BaseStream.Length - offset);
			return new BytecodeReader(_buffer, _offset + offset, count.Value);
		}
	}
}