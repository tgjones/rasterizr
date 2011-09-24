using System;
using System.Collections.Generic;
using Fasterflect;
using Nexus;

namespace Rasterizr.Core.Rasterizer.Interpolation
{
	public static class Interpolator
	{
		private static readonly Dictionary<Type, CachedValueInterpolator> CachedInterpolators;

		static Interpolator()
		{
			CachedInterpolators = new Dictionary<Type, CachedValueInterpolator>
			{
				{ typeof(float),    CreateCachedValueInterpolator<FloatInterpolator, float>() }, 
				{ typeof(ColorF),   CreateCachedValueInterpolator<ColorFInterpolator, ColorF>() }, 
				{ typeof(Point2D),  CreateCachedValueInterpolator<Point2DInterpolator, Point2D>() }, 
				{ typeof(Point3D),  CreateCachedValueInterpolator<Point3DInterpolator, Point3D>() }, 
				{ typeof(Point4D),  CreateCachedValueInterpolator<Point4DInterpolator, Point4D>() }, 
				{ typeof(Vector3D), CreateCachedValueInterpolator<Vector3DInterpolator, Vector3D>() }
			};
		}

		private static CachedValueInterpolator CreateCachedValueInterpolator<TValueInterpolator, TValue>() 
			where TValueInterpolator : IValueInterpolator, new()
		{
			return new CachedValueInterpolator
			{
				ValueInterpolator = new TValueInterpolator(),
				PerspectiveInvoker = GetPerspectiveInvoker<TValueInterpolator, TValue>(),
				LinearInvoker = GetLinearInvoker<TValueInterpolator, TValue>()
			};
		}

		private static MethodInvoker GetPerspectiveInvoker<TValueInterpolator, TValue>()
		{
			return typeof(TValueInterpolator).DelegateForCallMethod("Perspective",
				typeof(float), typeof(float), typeof(float),
				typeof(TValue), typeof(TValue), typeof(TValue),
				typeof(float), typeof(float), typeof(float));
		}

		private static MethodInvoker GetLinearInvoker<TValueInterpolator, TValue>()
		{
			return typeof(TValueInterpolator).DelegateForCallMethod("Linear",
				typeof(float), typeof(float), typeof(float),
				typeof(TValue), typeof(TValue), typeof(TValue));
		}

		public static object Linear(float alpha, float beta, float gamma,
		   object p1, object p2, object p3)
		{
			var cachedInterpolator = FindCachedInterpolator(p1.GetType());
			return cachedInterpolator.LinearInvoker(cachedInterpolator.ValueInterpolator, alpha, beta, gamma, p1, p2, p3);
		}

		public static object Perspective(float alpha, float beta, float gamma,
			object p1, object p2, object p3,
			float p1W, float p2W, float p3W)
		{
			var cachedInterpolator = FindCachedInterpolator(p1.GetType());
			return cachedInterpolator.PerspectiveInvoker(cachedInterpolator.ValueInterpolator, alpha, beta, gamma, p1, p2, p3, p1W, p2W, p3W);
		}

		private static CachedValueInterpolator FindCachedInterpolator(Type type)
		{
			CachedValueInterpolator valueInterpolator;
			if (!CachedInterpolators.TryGetValue(type, out valueInterpolator))
				throw new ArgumentException("Could not find value interpolator matching type", "type");
			return valueInterpolator;
		}

		private struct CachedValueInterpolator
		{
			public IValueInterpolator ValueInterpolator { get; set; }
			public MethodInvoker PerspectiveInvoker { get; set; }
			public MethodInvoker LinearInvoker { get; set; }
		}
	}
}