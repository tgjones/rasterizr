using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rasterizr
{
	public class RasterizrException : ApplicationException
	{
		public RasterizrException(string message)
			: base(message)
		{
		}
	}
}
