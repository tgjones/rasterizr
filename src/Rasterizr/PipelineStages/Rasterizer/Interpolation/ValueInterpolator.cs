namespace Rasterizr.PipelineStages.Rasterizer.Interpolation
{
	public abstract class ValueInterpolator<T> : IValueInterpolator
	{
		public abstract T Linear(float alpha, float beta, float gamma, 
			T p1, T p2, T p3);

		public abstract T Perspective(float alpha, float beta, float gamma, 
			T p1, T p2, T p3, 
			float p1W, float p2W, float p3W);
	}
}