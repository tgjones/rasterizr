using System;

namespace Rasterizr.Core
{
	public class RasterizrException : ApplicationException
	{
		public RasterizrException(string message)
			: base(message)
		{
		}
	}
}
