using System;

namespace Rasterizr.Core.Rasterizer
{
	public class TopLeftFillConvention : IFillConvention
	{
		/// <summary>
		/// Desired behaviour:
		/// 0.0 => 0
		/// 0.1 => 0
		/// 0.5 => 0
		/// 0.6 => 1
		/// 1.0 => 1
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int GetTopOrLeft(float value)
		{
			return (int) Math.Ceiling(value - 0.5f);
		}

		/// <summary>
		/// Desired behaviour:
		/// 1.0 => 0
		/// 1.1 => 0
		/// 1.5 => 0
		/// 1.6 => 1
		/// 2.0 => 1
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int GetBottomOrRight(float value)
		{
			return (int) Math.Ceiling(value - 0.5f) - 1;
		}
	}
}