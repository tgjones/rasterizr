using SlimShader.IO;

namespace SlimShader.Parser
{
	public abstract class BytecodeParser<TToken>
	{
		private readonly BytecodeReader _reader;

		protected BytecodeReader Reader
		{
			get { return _reader; }
		}

		protected BytecodeParser(BytecodeReader reader)
		{
			_reader = reader;
		}

		public abstract TToken Parse();
	}
}