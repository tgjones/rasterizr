using System.Diagnostics.Contracts;

namespace Rasterizr.Pipeline.Rasterizer
{
	public struct RasterizerStateDescription
	{
		public static RasterizerStateDescription Default
		{
			get
			{
				return new RasterizerStateDescription
				{
					FillMode = FillMode.Solid,
					CullMode = CullMode.Back,
					IsFrontCounterClockwise = false,
					DepthBias = 0,
					SlopeScaledDepthBias = 0.0f,
					DepthBiasClamp = 0.0f,
					IsDepthClipEnabled = true,
					IsScissorEnabled = false,
					IsMultisampleEnabled = false,
					IsAntialiasedLineEnabled = false
				};
			}
		}

		public FillMode FillMode;
		public CullMode CullMode;
		public bool IsFrontCounterClockwise;
		public int DepthBias;
		public float SlopeScaledDepthBias;
		public float DepthBiasClamp;
		public bool IsDepthClipEnabled;
		public bool IsScissorEnabled;
		public bool IsMultisampleEnabled;
		public bool IsAntialiasedLineEnabled;

        [Pure]
        internal bool ShouldCull(bool counterClockwise)
        {
            // If triangle is CW, then cull if:
            // (a) front faces are CW and we should cull front faces.
            // (b) front faces are CCW and we should cull back faces.
            return (IsFrontCounterClockwise == counterClockwise && CullMode == CullMode.Front)
                || (IsFrontCounterClockwise != counterClockwise && CullMode == CullMode.Back);
        }
	}
}