struct VS_IN
{
    float3 position : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD;
};

struct GS_CUBE_IN
{
    float3 position : TEXCOORD0;
    float3 worldPosition: TEXCOORD1;
    float3 normal : TEXCOORD2;
    float2 uv : TEXCOORD3;
};

struct PS_CUBE_IN
{
    float4 position : SV_POSITION;
    float3 worldPosition : TEXCOORD0;
    float3 normal : TEXCOORD1;
    float2 uv : TEXCOORD2;
    uint RTIndex : SV_RenderTargetArrayIndex;
};

struct PS_IN
{
    float4 position : SV_POSITION;
    float3 worldPosition: TEXCOORD0;
    float3 normal : TEXCOORD1;
    float2 uv : TEXCOORD2;
};

cbuffer VertexConstantData
{
	float4x4 WorldViewProjection;
	float4x4 World;
}

cbuffer GeometryConstantData
{
    float4x4 TransformMatrixArray[6];
}

cbuffer PixelConstantData
{
	float4 LightDirection;
    float4 EyePosition;
}

Texture2D DiffuseTexture;
SamplerState DiffuseSampler;
TextureCube CubeMap;

GS_CUBE_IN VS_CubeMap(VS_IN input)
{
    GS_CUBE_IN output = (GS_CUBE_IN) 0;
	
    output.position = input.position;
    output.worldPosition = mul(World, input.position);
	output.normal = mul(World, input.normal);
	output.uv = input.uv;
	
	return output;
}

[maxvertexcount(24)]
void GS_CubeMap(triangle GS_CUBE_IN input[3], inout TriangleStream<PS_CUBE_IN> CubeMapStream)
{
    for (int f = 0; f < 6; f++)
    {
        // Compute screen coordinates
        PS_CUBE_IN output;
        output.RTIndex = f;
        for (int v = 0; v < 3; v++)
        {
            output.position = mul(TransformMatrixArray[f], float4(input[v].position, 1));
            output.worldPosition = input[v].worldPosition;
            output.uv = input[v].uv;
            output.normal = input[v].normal;
            CubeMapStream.Append(output);
        }
        CubeMapStream.RestartStrip();
    }
}

float4 PS_CubeMap(PS_CUBE_IN input) : SV_Target
{
    float3 L = normalize(LightDirection.xyz);
    float D = saturate(dot(input.normal, L));

    float3 diffuseT = DiffuseTexture.Sample(DiffuseSampler, input.uv).rgb;
    return float4(diffuseT * D, 1);
}

PS_IN VS_Standard(VS_IN input)
{
    PS_IN output = (PS_IN) 0;

    output.position = mul(WorldViewProjection, float4(input.position, 1));
    output.worldPosition = mul(World, input.position);
    output.normal = mul(World, input.normal);
    output.uv = input.uv;

    return output;
}

float4 PS_Standard(PS_IN input) : SV_Target
{
    float3 L = normalize(LightDirection.xyz);
    float D = saturate(dot(input.normal, L));

    float3 diffuseT = DiffuseTexture.Sample(DiffuseSampler, input.uv).rgb;
    return float4(diffuseT * D, 1);
}

float4 PS_Reflective(PS_IN input) : SV_Target
{
    float3 N = normalize(input.normal);
    //store Light vector
    float3 L = normalize(LightDirection.xyz);

    //diffuse light	
    float D = saturate(dot(N, L));

    //specular light	
    float power = 3;
    float3 V = normalize(EyePosition.xyz - input.worldPosition.xyz);
    float3 R = normalize(2.0f * N * dot(N, L) - L);
    float3 S = pow(max(0.0f, dot(R, V)), power);

    //reflection
    float3 reflection = normalize(reflect(-V, N));

    //final light
    float3 diffuseT = DiffuseTexture.Sample(DiffuseSampler, input.uv).rgb;
    float3 refleColor = CubeMap.Sample(DiffuseSampler, reflection).rgb;

    return float4(lerp(diffuseT * D + S, refleColor, 0.7f), 1);
}