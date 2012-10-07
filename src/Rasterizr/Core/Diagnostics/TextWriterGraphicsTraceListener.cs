using System.IO;

namespace Rasterizr.Core.Diagnostics
{
	public class TextWriterGraphicsTraceListener : GraphicsTraceListener
	{
		private readonly TextWriter _writer;
		private int _primitiveIndex;

		public TextWriterGraphicsTraceListener(TextWriter writer)
		{
			_writer = writer;
		}

		public override void BeginApiCall(string methodName, params object[] methodArguments)
		{
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

		public override void EndApiCall()
		{
			_primitiveIndex = 0;
		}

		public override void Close()
		{
			_writer.Close();
		}
	}
}