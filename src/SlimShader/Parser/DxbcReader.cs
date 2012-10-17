using System;
using System.IO;

namespace SlimShader.Parser
{
	/// <summary>
	/// Reads tokens from a DirectX Bytecode file, and keeps track of the current position.
	/// </summary>
	public class DxbcReader
	{
		private readonly uint[] _tokens;

		public long CurrentIndex { get; private set; }

		public uint CurrentToken
		{
			get { return _tokens[CurrentIndex]; }
		}

		public uint NextToken
		{
			get { return _tokens[CurrentIndex + 1]; }
		}

		public uint this[int index]
		{
			get { return _tokens[index]; }
		}

		public DxbcReader(BinaryReader binaryReader)
		{
			var length = binaryReader.BaseStream.Length;
			var tokenLength = ConvertByteToUIntOffset(length);
			_tokens = new uint[tokenLength];
			for (int i = 0; i < tokenLength; i++)
				_tokens[i] = binaryReader.ReadUInt32();
			CurrentIndex = 0;
		}

		private long ConvertByteToUIntOffset(long offset)
		{
			return offset / sizeof(uint);
		}

		public uint ReadAndMoveNext()
		{
			var token = CurrentToken;
			MoveNext();
			return token;
		}

		public void MoveNext()
		{
			CurrentIndex += 1;
		}

		public void Seek(long offsetInBytes, SeekOrigin origin)
		{
			if (offsetInBytes % 4 != 0)
				throw new ArgumentOutOfRangeException("offsetInBytes", "Offset must be a multiple of 4");
			long offset = offsetInBytes / 4;

			switch (origin)
			{
				case SeekOrigin.Begin:
					CurrentIndex = offset;
					break;
				case SeekOrigin.Current:
					CurrentIndex += offset;
					break;
				case SeekOrigin.End:
					CurrentIndex = _tokens.Length - offset;
					break;
				default:
					throw new ArgumentOutOfRangeException("origin");
			}
		}
	}
}