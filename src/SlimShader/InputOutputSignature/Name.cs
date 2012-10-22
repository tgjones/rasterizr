using System.ComponentModel;

namespace SlimShader.InputOutputSignature
{
	/// <summary>
	/// Values that identify shader parameters that use system-value semantics.
	/// Based on D3D_NAME. This is similar to, but different from, <see cref="SlimShader.Shader.SystemValueName"/>
	/// </summary>
	public enum Name
	{
		[Description("NONE")]
		Undefined = 0,

		[Description("POS")]
		Position = 1,

		ClipDistance = 2,
		CullDistance = 3,
		RenderTargetArrayIndex = 4,
		ViewportArrayIndex = 5,
		VertexID = 6,
		PrimitiveID = 7,
		InstanceID = 8,
		IsFrontFace = 9,
		SampleIndex = 10,
		FinalQuadEdgeTessFactor = 11,
		FinalQuadInsideTessFactor = 12,
		FinalTriEdgeTessFactor = 13,
		FinalTriInsideTessFactor = 14,
		FinalLineDetailTessFactor = 15,
		FinalLineDensityTessFactor = 16,

		[Description("TARGET")]
		Target = 64,
		Depth = 65,
		Coverage = 66,
		DepthGreaterEqual = 67,
		DepthLessEqual = 68
	}
}