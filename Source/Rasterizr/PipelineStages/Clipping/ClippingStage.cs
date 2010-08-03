using System.Collections.Generic;
using Rasterizr.PipelineStages.PrimitiveAssembly;
using Rasterizr.PipelineStages.ShaderStages.VertexShader;

namespace Rasterizr.PipelineStages.Clipping
{
	public class ClippingStage : PipelineStageBase<TrianglePrimitive, TrianglePrimitive>
	{
		#region Fields

		private const float CLIP_LEFT = -0.9f;
		private const float CLIP_RIGHT = 0.9f;
		private const float CLIP_TOP = 0.9f;
		private const float CLIP_BOTTOM = -0.9f;
		private const float CLIP_NEAR = -1f;
		private const float CLIP_FAR = 1f;

		#endregion

		public override void Process(IList<TrianglePrimitive> inputs, IList<TrianglePrimitive> outputs)
		{
			for (int i = 0; i < inputs.Count; ++i)
			{
				// Clip triangle using Blinn's clipping technique.
				ClipTriangle(inputs[i], outputs);
			}

			// TODO: Clip against user-defined planes.
		}

		private void ClipTriangle(TrianglePrimitive triangle, IList<TrianglePrimitive> results)
		{
			// Calculate clip regions for each of the three vertices.
			ClipRegions p1Regions = GetClipRegions(triangle.V1);
			ClipRegions p2Regions = GetClipRegions(triangle.V2);
			ClipRegions p3Regions = GetClipRegions(triangle.V3);

			// Trivial reject.
			if (p1Regions != ClipRegions.NotClipped && p2Regions != ClipRegions.NotClipped && p3Regions != ClipRegions.NotClipped)
				return;

			// Trivial accept.
			if (p1Regions == ClipRegions.NotClipped && p2Regions == ClipRegions.NotClipped && p3Regions == ClipRegions.NotClipped)
			{
				results.Add(triangle);
				return;
			}

			// Non-trivial cases.
			for (int i = 0; i < 2; ++i)
			{
				float num = CLIP_LEFT - 
			}
			/*
			 * //Non-trivial cases
  in_vertices.push_back(p1);
  in_vertices.push_back(p2);
  in_vertices.push_back(p3);
  
  //Clipping against left edge
  for (i=0; i<in_vertices.size() - 1; i++)
    {
      num = CLIP_LEFT - in_vertices[i].clipCoords[0];
      denom = in_vertices[i+1].clipCoords[0] - in_vertices[i].clipCoords[0];
      clipLineSegment(num, denom, in_vertices[i], in_vertices[i+1], out_vertices);
    }
  num = CLIP_LEFT - in_vertices.back().clipCoords[0];
  denom = in_vertices.front().clipCoords[0] - in_vertices.back().clipCoords[0];
  clipLineSegment(num,denom, in_vertices.back(), in_vertices.front(), out_vertices);

  if (out_vertices.size() == 0) return false;
  in_vertices = out_vertices;
  out_vertices.clear();

  //Clipping against right edge
  for (i=0; i<in_vertices.size() - 1; i++)
    {
      num = in_vertices[i].clipCoords[0] - CLIP_RIGHT;
      denom = in_vertices[i].clipCoords[0]-in_vertices[i+1].clipCoords[0];
      clipLineSegment(num, denom, in_vertices[i], in_vertices[i+1], out_vertices);
    }
  num = in_vertices.back().clipCoords[0] - CLIP_RIGHT;
  denom = in_vertices.back().clipCoords[0] - in_vertices.front().clipCoords[0];
  clipLineSegment(num,denom, in_vertices.back(), in_vertices.front(), out_vertices);

  if (out_vertices.size() == 0) return false;
  in_vertices = out_vertices;
  out_vertices.clear();

  //Clipping against top edge
  for (i=0; i<in_vertices.size() - 1; i++)
    {
      num = in_vertices[i].clipCoords[1] - CLIP_TOP;    
      denom = in_vertices[i].clipCoords[1]-in_vertices[i+1].clipCoords[1];        
      clipLineSegment(num, denom, in_vertices[i], in_vertices[i+1], out_vertices);
    }
  num = in_vertices.back().clipCoords[1] - CLIP_TOP;
  denom = in_vertices.back().clipCoords[1] - in_vertices.front().clipCoords[1];
  clipLineSegment(num,denom, in_vertices.back(), in_vertices.front(), out_vertices);

  if (out_vertices.size() == 0) return false;
  in_vertices = out_vertices;
  out_vertices.clear();


  //Clipping against bottom edge
  for (i=0; i< in_vertices.size() - 1; i++) //PROBLEM! size() returns an unsigned int. If this is 0 then when we subtract 1 we get a very high number!!!
                                            //FIXED by checking if out_vertices are empty at any time
    {
      num = CLIP_BOTTOM - in_vertices[i].clipCoords[1];    
      denom = in_vertices[i+1].clipCoords[1]-in_vertices[i].clipCoords[1];
      clipLineSegment(num, denom, in_vertices[i], in_vertices[i+1], out_vertices);
    }

  num = CLIP_BOTTOM - in_vertices.back().clipCoords[1];
  denom = in_vertices.front().clipCoords[1] - in_vertices.back().clipCoords[1];
  clipLineSegment(num,denom, in_vertices.back(), in_vertices.front(), out_vertices);

  if (out_vertices.size() == 0) return false;
  in_vertices = out_vertices;
  out_vertices.clear();

  
  //Clipping against far edge
  for (i=0; i<in_vertices.size() - 1; i++)
    {
      num = in_vertices[i].clipCoords[2] - CLIP_FAR;    
      denom = in_vertices[i].clipCoords[2] - in_vertices[i+1].clipCoords[2];        
      clipLineSegment(num, denom, in_vertices[i], in_vertices[i+1], out_vertices);
    }
  num = in_vertices.back().clipCoords[2] - CLIP_FAR;
  denom = in_vertices.back().clipCoords[2] - in_vertices.front().clipCoords[2];
  clipLineSegment(num,denom, in_vertices.back(), in_vertices.front(), out_vertices);


  if (out_vertices.size() == 0) return false;
  in_vertices = out_vertices;
  out_vertices.clear();

  //Clipping against near edge
  for (i=0; i<in_vertices.size() - 1; i++)
    {
      num = CLIP_NEAR - in_vertices[i].clipCoords[2];
      denom = in_vertices[i+1].clipCoords[2] - in_vertices[i].clipCoords[2];
      clipLineSegment(num, denom, in_vertices[i], in_vertices[i+1], out_vertices);
    }
  num = CLIP_NEAR - in_vertices.back().clipCoords[2];
  denom = in_vertices.front().clipCoords[2] - in_vertices.back().clipCoords[2];
  clipLineSegment(num,denom, in_vertices.back(), in_vertices.front(), out_vertices);



  if (out_vertices.size() == 0) return false;

  result = new TriangleFan(out_vertices, out_vertices.size());

  return true;
			 * */
		}

		private static ClipRegions GetClipRegions(VertexShaderOutput v)
		{
			ClipRegions result = ClipRegions.NotClipped;

			if (v.Position.X < CLIP_LEFT) result = ClipRegions.Left;
			else if (v.Position.X > CLIP_RIGHT) result = ClipRegions.Right;

			if (v.Position.Y > CLIP_TOP) result |= ClipRegions.Top;
			else if (v.Position.Y < CLIP_BOTTOM) result |= ClipRegions.Bottom;

			if (v.Position.Z < CLIP_NEAR) result |= ClipRegions.Near;
			else if (v.Position.Z > CLIP_FAR) result |= ClipRegions.Far;

			return result;
		}

		private void ClipLineSegment(float num, float denom, VertexShaderOutput p1, VertexShaderOutput p2, List<VertexShaderOutput> outputVertices)
		{
			if (denom > 0 || denom < 0)
			{
				// denom > 0 = potentially entering
				// denom < 0 = potentially leaving
				float t = num / denom;
				if (t >= 0 && t <= 1)
				{
					//float one_div_w = p1.one_div_w * (1 - t) + p2.one_div_w * t;

					// Interpolate vertex attributes between p1 and p2 using t.
					VertexShaderOutput newVertex = new VertexShaderOutput
					{
						Position = p1.Position + (p2.Position - p1.Position) * t,
						Attributes = new VertexAttributes.VertexAttribute[p1.Attributes.Length]
					};
					for (int i = 0; i < p1.Attributes.Length; ++i)
					{
						newVertex.Attributes[i] = new VertexAttributes.VertexAttribute
						{
							Name = p1.Attributes[i].Name,
							InterpolationType = p1.Attributes[i].InterpolationType,
							Value = p1.Attributes[i].Value.Add(p2.Attributes[i].Value.Subtract(p1.Attributes[i].Value).Multiply(t))
						};
					}

					outputVertices.Add(newVertex);
					if (denom > 0)
						outputVertices.Add(p2);
				}
				else if (num <= 0)
				{
					//Both points inside
					outputVertices.Add(p2);
				}
			}
			else if (num <= 0)
			{
				// Both points inside
				outputVertices.Add(p2);

			} //else add nothing
		}
	}
}