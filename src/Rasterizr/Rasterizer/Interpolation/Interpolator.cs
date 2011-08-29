using System;
using System.Collections.Generic;
using Fasterflect;
using Nexus;

namespace Rasterizr.Rasterizer.Interpolation
{
	public static class Interpolator
	{
		private static readonly Dictionary<Type, IValueInterpolator> ValueInterpolators;

		static Interpolator()
		{
			ValueInterpolators = new Dictionary<Type, IValueInterpolator>
			{
				{ typeof(float), new FloatInterpolator() }, 
				{ typeof(ColorF), new ColorFInterpolator() }, 
				{ typeof(Point2D), new Point2DInterpolator() }, 
				{ typeof(Point3D), new Point3DInterpolator() }, 
				{ typeof(Point4D), new Point4DInterpolator() }, 
				{ typeof(Vector3D), new Vector3DInterpolator() }
			};
		}

		public static object Linear(float alpha, float beta, float gamma,
		   object p1, object p2, object p3)
		{
			// TODO: Use Cache API
			return FindInterpolator(p1.GetType()).CallMethod("Linear", 
				alpha, beta, gamma, p1, p2, p3);
		}

		public static object Perspective(float alpha, float beta, float gamma,
			object p1, object p2, object p3,
			float p1W, float p2W, float p3W)
		{
			// TODO: Use Cache API
			return FindInterpolator(p1.GetType()).CallMethod("Perspective", 
				alpha, beta, gamma, p1, p2, p3, p1W, p2W, p3W);
		}

		private static IValueInterpolator FindInterpolator(Type type)
		{
			IValueInterpolator valueInterpolator;
			if (!ValueInterpolators.TryGetValue(type, out valueInterpolator))
				throw new ArgumentException("Could not find value interpolator matching type", "type");
			return valueInterpolator;
		}
	}
}