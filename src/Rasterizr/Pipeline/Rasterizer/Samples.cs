using System;

namespace Rasterizr.Pipeline.Rasterizer
{
	public struct Samples
	{
		public bool AnyCovered;
		public Sample Sample0;
		public Sample Sample1;
		public Sample Sample2;
		public Sample Sample3;

		public Sample this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return Sample0;
					case 1 :
						return Sample1;
					case 2 :
						return Sample2;
					case 3 :
						return Sample3;
					default :
						throw new ArgumentOutOfRangeException("index");
				}
			}
		}
	}
}