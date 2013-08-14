struct PixelShaderInput
{
	float4 pos : SV_POSITION;
	float3 worldPos: TEXCOORD0;
	float3 normal : TEXCOORD1;
	float2 uv : TEXCOORD2;
};