using System.IO;

namespace Rasterizr.Core.Diagnostics
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

		protected internal override void EndFrame()
		{
			_writtenBeginFrame = false;
		}

		protected internal override void BeginApiCall(string methodName, params object[] methodArguments)
		{
			if (!_writtenBeginFrame)
			{
				_writer.WriteLine("Frame " + _frameNumber++);
				_writtenBeginFrame = true;
			}

			_writer.Write("- {0}", methodName);
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
		}
	}
}