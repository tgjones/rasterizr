using System;
using Rasterizr.Math;

namespace Rasterizr.Pipeline.Rasterizer
{
	internal static class MultiSamplingUtility
	{
        public static Point GetSamplePosition(int multiSampleCount, int x, int y, int sampleIndex)
		{
			switch (multiSampleCount)
			{
				case 1:
                    return new Point(x + 0.5f, y + 0.5f);
				case 2:
					switch (sampleIndex)
					{
						case 0:
                            return new Point(x + 0.25f, y + 0.25f);
						case 1:
                            return new Point(x + 0.75f, y + 0.75f);
						default:
							throw new InvalidOperationException();
					}
				case 4:
					switch (sampleIndex)
					{
						case 0:
                            return new Point(x + 0.31f, y + 0.11f);
						case 1:
                            return new Point(x + 0.88f, y + 0.31f);
						case 2:
                            return new Point(x + 0.68f, y + 0.88f);
						case 3:
                            return new Point(x + 0.11f, y + 0.68f);
						default:
							throw new InvalidOperationException();
					}
				default:
					throw new ArgumentOutOfRangeException("multiSampleCount");
			}
		}
	}
}