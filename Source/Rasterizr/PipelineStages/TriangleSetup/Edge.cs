namespace Rasterizr.PipelineStages.TriangleSetup
{
	public struct Edge
	{
		/// <summary>
		/// Fractional x
		/// </summary>
		public float X;

		/// <summary>
		/// Fractional dX/dY
		/// </summary>
		public float XStep;

		public int Y;
		public int Height;

		/// <summary>
		/// 1/w
		/// </summary>
		public float OneOverW;
		public float OneOverWStep;

		public VertexAttributeStepValue[] VertexAttributeStepValues;

		public Edge(float[] xCoords, float[] yCoords, int top, int bottom,
			TriangleGradients triangleGradients, IFillConvention fillConvention)
		{
			Y = fillConvention.GetTopOrLeft(yCoords[top]);
			int yEnd = fillConvention.GetBottomOrRight(yCoords[bottom]);
			Height = yEnd - Y;

			float yPreStep = Y + 0.5f - yCoords[top];

			float realHeight = yCoords[bottom] - yCoords[top];
			float realWidth = xCoords[bottom] - xCoords[top];

			X = ((realWidth * yPreStep) / realHeight) + xCoords[top];
			XStep = realWidth / realHeight;

			float xPreStep = X - xCoords[top];

			OneOverW = triangleGradients.InterpolatedVertexAttributes[top].OneOverW
				+ yPreStep * triangleGradients.DOneOverWdY
				+ xPreStep * triangleGradients.DOneOverWdX;
			OneOverWStep = XStep * triangleGradients.DOneOverWdX
				+ triangleGradients.DOneOverWdY;

			VertexAttributeStepValues = new VertexAttributeStepValue[triangleGradients.VertexAttributeStepValues.Length];
			for (int i = 0; i < VertexAttributeStepValues.Length; ++i)
			{
				VertexAttributeStepValues[i].Value = triangleGradients.InterpolatedVertexAttributes[top].Attributes[i].Value
					.Add(triangleGradients.VertexAttributeStepValues[i].YStep.Multiply(yPreStep))
					.Add(triangleGradients.VertexAttributeStepValues[i].XStep.Multiply(xPreStep));
				VertexAttributeStepValues[i].ValueStep = triangleGradients.VertexAttributeStepValues[i].XStep.Multiply(XStep)
					.Add(triangleGradients.VertexAttributeStepValues[i].YStep);
			}
		}

		/// <summary>
		/// Normally I don't like to have mutable structs, but for performance
		/// reasons I prefer Edge as a struct than a class.
		/// </summary>
		public void Step()
		{
			X += XStep;
			Y++;
			Height--;
			OneOverW += OneOverWStep;

			for (int i = 0; i < VertexAttributeStepValues.Length; ++i)
				VertexAttributeStepValues[i].Step();
		}
	}
}