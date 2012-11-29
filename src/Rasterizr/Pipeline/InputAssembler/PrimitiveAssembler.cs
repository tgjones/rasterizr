using System;
using System.Collections.Generic;
using Rasterizr.Pipeline.VertexShader;

namespace Rasterizr.Pipeline.InputAssembler
{
	internal static class PrimitiveAssembler
	{
		public static IEnumerable<InputAssemblerPrimitiveOutput> GetPrimitiveStream( 
			IEnumerable<VertexShaderOutput> shadedVertices,
			PrimitiveTopology primitiveTopology)
		{
			var enumerator = shadedVertices.GetEnumerator();
			var primitiveID = 0;
			switch (primitiveTopology)
			{
				case PrimitiveTopology.PointList:
					{
						while (enumerator.MoveNext())
							yield return new InputAssemblerPrimitiveOutput
							{
								PrimitiveID = primitiveID++,
								Vertices = new[] { enumerator.Current },
							};
						break;
					}
				case PrimitiveTopology.LineList:
					{
						while (enumerator.MoveNext())
						{
							var vertex0 = enumerator.Current;
							enumerator.MoveNext();
							var vertex1 = enumerator.Current;
							yield return new InputAssemblerPrimitiveOutput
							{
								PrimitiveID = primitiveID++,
								Vertices = new[] { vertex0, vertex1 }
							};
						}
						break;
					}
				case PrimitiveTopology.LineStrip:
					{
						enumerator.MoveNext();
						var vertex0 = enumerator.Current;
						while (enumerator.MoveNext())
						{
							var vertex1 = enumerator.Current;
							if (vertex1.IsStripCut)
							{
								if (enumerator.MoveNext())
									vertex0 = enumerator.Current;
							}
							else
							{
								yield return new InputAssemblerPrimitiveOutput
								{
									PrimitiveID = primitiveID++,
									Vertices = new[] { vertex0, vertex1 }
								};
								vertex0 = vertex1;
							}
						}
						break;
					}
				case PrimitiveTopology.TriangleList:
					{
						while (enumerator.MoveNext())
						{
							var vertex0 = enumerator.Current;
							enumerator.MoveNext();
							var vertex1 = enumerator.Current;
							enumerator.MoveNext();
							var vertex2 = enumerator.Current;
							yield return new InputAssemblerPrimitiveOutput
							{
								PrimitiveID = primitiveID++,
								Vertices = new[] { vertex0, vertex1, vertex2 }
							};
						}
						break;
					}
				case PrimitiveTopology.TriangleStrip:
					{
						enumerator.MoveNext();
						var vertex0 = enumerator.Current;
						enumerator.MoveNext();
						var vertex1 = enumerator.Current;
						while (enumerator.MoveNext())
						{
							var vertex2 = enumerator.Current;
							if (vertex2.IsStripCut)
							{
								if (enumerator.MoveNext())
									vertex0 = enumerator.Current;
								if (enumerator.MoveNext())
									vertex1 = enumerator.Current;
							}
							else
							{
								yield return new InputAssemblerPrimitiveOutput
								{
									PrimitiveID = primitiveID++,
									Vertices = new[] { vertex0, vertex1, vertex2 }
								};
								vertex1 = vertex2;
								vertex0 = vertex1;
							}
						}
						break;
					}
				case PrimitiveTopology.LineListWithAdjacency:
					break;
				case PrimitiveTopology.LineStripWithAdjacency:
					break;
				case PrimitiveTopology.TriangleListWithAdjacency:
					break;
				case PrimitiveTopology.TriangleStripWithAdjacency:
					break;
				default:
					throw new ArgumentOutOfRangeException("primitiveTopology");
			}
		}
	}
}