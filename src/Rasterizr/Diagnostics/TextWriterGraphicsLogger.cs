using System.IO;

namespace Rasterizr.Diagnostics
{
	public class TextWriterGraphicsLogger : GraphicsLogger
	{
		private readonly TextWriter _writer;
		private bool _writtenBeginFrame;
		private int _frameNumber;

		public TextWriterGraphicsLogger(TextWriter writer)
		{
			_writer = writer;
		}

		protected internal override void BeginOperation(OperationType type, params object[] methodArguments)
		{
			if (!_writtenBeginFrame)
			{
				_writer.WriteLine("Frame " + _frameNumber++);
				_writtenBeginFrame = true;
			}

			_writer.Write("- {0}", type);
			if (methodArguments != null && methodArguments.Length > 0)
			{
				_writer.Write("(");
				for (int i = 0; i < methodArguments.Length; i++)
				{
					_writer.Write(methodArguments[i]);
					if (i < methodArguments.Length - 1)
						_writer.Write(", ");
				}
				_writer.Write(")");
			}
			_writer.WriteLine();

			if (type == OperationType.SwapChainPresent)
				_writtenBeginFrame = false;
		}
	}
}