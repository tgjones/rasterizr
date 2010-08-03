namespace Rasterizr.PipelineStages.TriangleSetup
{
	public interface IFillConvention
	{
		int GetTopOrLeft(float value);
		int GetBottomOrRight(float value);
	}
}