using System;
using System.Collections.Generic;
using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.InputAssembler
{
	internal struct InputAssemblerPrimitiveOutput
	{
		public PrimitiveType PrimitiveType;
		public int PrimitiveID;

		public VertexShaderOutput Vertex0;
		public VertexShaderOutput Vertex1;
		public VertexShaderOutput Vertex2;

		public IEnumerable<VertexShaderOutput> Vertices
		{
			get
			{
				switch (PrimitiveType)
				{
					case PrimitiveType.Point:
						yield return Vertex0;
						break;
					case PrimitiveType.Line:
						yield return Vertex0;
						yield return Vertex1;
						break;
					case PrimitiveType.Triangle:
						yield return Vertex0;
						yield return Vertex1;
						yield return Vertex2;
						break;
					case PrimitiveType.PointWithAdjacency:
						break;
					case PrimitiveType.LineWithAdjacency:
						break;
					case PrimitiveType.TriangleWithAdjacency:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}