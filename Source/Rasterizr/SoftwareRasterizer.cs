namespace Apollo.Graphics.Rendering.Rasterization.SoftwareRasterizer
{
	public class SoftwareRasterizer
	{
		#region Properties

		public RenderPipeline RenderPipeline { get; private set; }

		#endregion

		#region Constructor

		public SoftwareRasterizer()
		{
			RenderPipeline = new RenderPipeline();
		}

		#endregion

		public void BeginFrame()
		{
			RenderPipeline.Clear();
		}

		/*public override void DrawMesh(Mesh mesh, Matrix3D worldMatrix)
		{
			RenderPipeline.Draw();
		}*/

		public void EndFrame()
		{
			
		}
	}
}