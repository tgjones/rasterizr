using System.Collections.Generic;
using Rasterizr.PipelineStages.PerspectiveDivide;
using Rasterizr.VertexAttributes;

namespace Rasterizr.PipelineStages.TriangleSetup
{
	public class TriangleSetupStage : PipelineStageBase<ScreenSpaceVertex, Triangle>
	{
		public IFillConvention FillConvention { get; set; }

		public TriangleSetupStage()
		{
			FillConvention = new TopLeftFillConvention();
		}

		/// <summary>
		/// Here we are in screen space.
		/// </summary>
		/// <param name="inputs"></param>
		/// <param name="outputs"></param>
		public override void Process(IList<ScreenSpaceVertex> inputs, IList<Triangle> outputs)
		{
			// TODO: This will ultimately depend on the Input Assembler primitive topology -
			// triangle list, triangle strip, etc.
			for (int i = 0; i < inputs.Count; i += 3)
			{
				Triangle triangle;
				if (BuildTriangle(new[] { inputs[i], inputs[i + 1], inputs[i + 2] }, out triangle))
					outputs.Add(triangle);
			}
		}

		internal bool BuildTriangle(ScreenSpaceVertex[] vertices, out Triangle triangle)
		{
			TriangleSetupInfo triangleSetupInfo;
			return BuildTriangle(vertices, out triangle, out triangleSetupInfo);
		}

		internal bool BuildTriangle(ScreenSpaceVertex[] vertices, out Triangle triangle, out TriangleSetupInfo triangleSetupInfo)
		{
			float[] xCoords = new[] { vertices[0].Position.X, vertices[1].Position.X, vertices[2].Position.X };
			float[] yCoords = new[] { vertices[0].Position.Y, vertices[1].Position.Y, vertices[2].Position.Y };

			triangle = new Triangle();
			triangleSetupInfo = new TriangleSetupInfo { Gradients = new TriangleGradients(vertices) };
			triangle.DOneOverWdX = triangleSetupInfo.Gradients.DOneOverWdX;
			triangle.VertexAttributeXStepValues = new IVertexAttributeValue[triangleSetupInfo.Gradients.VertexAttributeStepValues.Length];
			triangle.VertexAttributeYStepValues = new IVertexAttributeValue[triangleSetupInfo.Gradients.VertexAttributeStepValues.Length];
			for (int i = 0; i < triangleSetupInfo.Gradients.VertexAttributeStepValues.Length; ++i)
			{
				triangle.VertexAttributeXStepValues[i] = triangleSetupInfo.Gradients.VertexAttributeStepValues[i].XStep;
				triangle.VertexAttributeYStepValues[i] = triangleSetupInfo.Gradients.VertexAttributeStepValues[i].YStep;
			}

			float y0 = yCoords[0];
			float y1 = yCoords[1];
			float y2 = yCoords[2];

			// Sort vertices in y.
			int top, middle, bottom;
			SortVerticesInY(y0, y1, y2, out top, out middle, out bottom);

			// Calculate determinant - this will tell us the triangle winding direction (CW or CCW).
			float dx0 = xCoords[middle] - xCoords[top];
			float dy0 = yCoords[middle] - yCoords[top];
			float dx1 = xCoords[bottom] - xCoords[top];
			float dy1 = yCoords[bottom] - yCoords[top];
			float determinant = dx0 * dy1 - dx1 * dy0;

			// If determinant > 0, midpoint is to the right.
			// If determinant < 0, midpoint is to the left.
			// If determinant = 0, triangle is degenerate.

			if (determinant == 0)
				return false; // degenerate triangle

			return ThreeBuffers(top, middle, bottom, xCoords, yCoords, ref triangle, ref triangleSetupInfo, determinant < 0);
		}

		private static void SortVerticesInY(float y0, float y1, float y2, out int top, out int middle, out int bottom)
		{
			if (y0 < y1)
			{
				if (y2 < y0)
				{
					top = 2;
					middle = 0;
					bottom = 1;
				}
				else
				{
					top = 0;
					if (y1 < y2)
					{
						middle = 1;
						bottom = 2;
					}
					else
					{
						middle = 2;
						bottom = 1;
					}
				}
			}
			else
			{
				if (y2 < y1)
				{
					top = 2;
					middle = 1;
					bottom = 0;
				}
				else
				{
					top = 1;
					if (y0 < y2)
					{
						middle = 0;
						bottom = 2;
					}
					else
					{
						middle = 2;
						bottom = 0;
					}
				}
			}
		}

		/// <summary>
		/// If the three y-values of the triangle vertices are distinct, two edges
		/// are processed to fill in one edge buffer, and the remaining edge is used
		/// to fill in the other edge buffer.
		/// </summary>
		private bool ThreeBuffers(int top, int middle, int bottom, float[] xCoords, float[] yCoords,
			ref Triangle rasterizedTriangle, ref TriangleSetupInfo triangleSetupInfo, bool midpointIsLeft)
		{
			Edge topToBottomEdge = new Edge(xCoords, yCoords, top, bottom, triangleSetupInfo.Gradients, FillConvention);
			Edge topToMiddleEdge = new Edge(xCoords, yCoords, top, middle, triangleSetupInfo.Gradients, FillConvention);
			Edge middleToBottomEdge = new Edge(xCoords, yCoords, middle, bottom, triangleSetupInfo.Gradients, FillConvention);

			triangleSetupInfo.TopToBottomEdge = topToBottomEdge;
			triangleSetupInfo.TopToMiddleEdge = topToMiddleEdge;
			triangleSetupInfo.MiddleToBottomEdge = middleToBottomEdge;

			List<Scanline> scanlines = new List<Scanline>();

			int height = topToMiddleEdge.Height;
			while (height-- >= 0)
			{
				Scanline scanline;
				if (BuildScanline(midpointIsLeft ? topToMiddleEdge : topToBottomEdge,
					midpointIsLeft ? topToBottomEdge : topToMiddleEdge, triangleSetupInfo.Gradients, out scanline))
					scanlines.Add(scanline);
				topToMiddleEdge.Step();
				topToBottomEdge.Step();
			}

			height = middleToBottomEdge.Height;
			while (height-- >= 0)
			{
				Scanline scanline;
				if (BuildScanline(midpointIsLeft ? middleToBottomEdge : topToBottomEdge,
					midpointIsLeft ? topToBottomEdge : middleToBottomEdge, triangleSetupInfo.Gradients, out scanline))
					scanlines.Add(scanline);
				middleToBottomEdge.Step();
				topToBottomEdge.Step();
			}

			rasterizedTriangle.Scanlines = scanlines.ToArray();

			if (rasterizedTriangle.Scanlines.Length > 0)
			{
				rasterizedTriangle.YTop = rasterizedTriangle.Scanlines[0].Y;
				rasterizedTriangle.YBottom = rasterizedTriangle.Scanlines[rasterizedTriangle.Scanlines.Length - 1].Y;
				return true;
			}

			return false;
		}

		private bool BuildScanline(Edge left, Edge right, TriangleGradients gradients, out Scanline scanline)
		{
			scanline = new Scanline();
			scanline.Y = left.Y;
			scanline.XStart = FillConvention.GetTopOrLeft(left.X);
			scanline.XPrestep = scanline.XStart - left.X;
			scanline.Width = FillConvention.GetBottomOrRight(right.X) - scanline.XStart;

			scanline.InterpolatedVertexAttributes = new InterpolatedVertexAttributes();
			scanline.InterpolatedVertexAttributes.OneOverW = left.OneOverW + scanline.XPrestep * gradients.DOneOverWdX;
			scanline.InterpolatedVertexAttributes.Attributes = new InterpolatedVertexAttribute[gradients.VertexAttributeStepValues.Length];
			for (int i = 0; i < scanline.InterpolatedVertexAttributes.Attributes.Length; ++i)
			{
				scanline.InterpolatedVertexAttributes.Attributes[i].Name = gradients.InterpolatedVertexAttributes[0].Attributes[i].Name;
				scanline.InterpolatedVertexAttributes.Attributes[i].Semantic = gradients.InterpolatedVertexAttributes[0].Attributes[i].Semantic;
				scanline.InterpolatedVertexAttributes.Attributes[i].InterpolationType = gradients.VertexAttributeStepValues[i].InterpolationType;
				scanline.InterpolatedVertexAttributes.Attributes[i].Value = left.VertexAttributeStepValues[i].Value
					.Add(gradients.VertexAttributeStepValues[i].XStep.Multiply(scanline.XPrestep));
			}

			return scanline.Width >= 0;
		}
	}
}