using System;

namespace Rasterizr
{
	public class RasterizrException : ApplicationException
	{
		public RasterizrException(string format, params object[] args)
			: this(string.Format(format, args))
		{
		}

		public RasterizrException(string message)
			: base(message)
		{
		}
	}
}
