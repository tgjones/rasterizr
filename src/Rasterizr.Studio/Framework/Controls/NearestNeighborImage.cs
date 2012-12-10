using System.Windows.Media;

namespace Rasterizr.Studio.Framework.Controls
{
	public class NearestNeighborImage : System.Windows.Controls.Image
	{
		protected override void OnRender(DrawingContext dc)
		{
			VisualBitmapScalingMode = BitmapScalingMode.NearestNeighbor;
			base.OnRender(dc);
		}
	}
}