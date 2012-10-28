namespace Rasterizr.Rasterizer
{
	public interface IFillConvention
	{
		int GetTopOrLeft(float value);
		int GetBottomOrRight(float value);
	}
}