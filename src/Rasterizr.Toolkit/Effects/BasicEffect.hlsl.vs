#include "BasicEffect.hlsl.h"

struct VertexShaderInput
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
};

float4x4 WorldViewProjection;

PixelShaderInput main(VertexShaderInput input)
{
	PixelShaderInput output = (PixelShaderInput) 0;
	
	output.pos = mul(input.pos, WorldViewProjection);
	output.tex = input.tex;
	
	return output;
}