using System.Runtime.InteropServices;
using Nexus;
using Rasterizr.Resources;
using Rasterizr.Util;

namespace Rasterizr.Toolkit.Effects
{
	public class BasicEffect : Effect
	{
	    private readonly Buffer _vertexShaderBuffer;
        private readonly Buffer _pixelShaderBuffer;

	    private VertexShaderData _vertexShaderData;
        private PixelShaderData _pixelShaderData;

        public Matrix3D World { get; set; }
        public Matrix3D View { get; set; }
        public Matrix3D Projection { get; set; }

        public Point3D LightPosition { get; set; }

        public Color3F DiffuseColor { get; set; }
        public float Alpha { get; set; }

		public BasicEffect(DeviceContext deviceContext)
			: base(deviceContext, BasicEffectCode.VertexShaderCode, BasicEffectCode.PixelShaderCode)
		{
		    _vertexShaderBuffer = deviceContext.Device.CreateBuffer(
		        new BufferDescription(BindFlags.ConstantBuffer)
		        {
		            SizeInBytes = Utilities.SizeOf<VertexShaderData>()
		        });
            _pixelShaderBuffer = deviceContext.Device.CreateBuffer(
                new BufferDescription(BindFlags.ConstantBuffer)
                {
                    SizeInBytes = Utilities.SizeOf<PixelShaderData>()
                });

		    Alpha = 1;
		}

        public override void Apply()
        {
            _vertexShaderData.WorldViewProjection = Matrix3D.Transpose(World * View * Projection);
            _vertexShaderData.World = Matrix3D.Transpose(World);

            _pixelShaderData.DiffuseColorAndAlpha = new Color4F(
                DiffuseColor.Red, DiffuseColor.Green, DiffuseColor.Blue,
                Alpha);
            _pixelShaderData.LightPos = LightPosition;

            _vertexShaderBuffer.SetData(ref _vertexShaderData);
            _pixelShaderBuffer.SetData(ref _pixelShaderData);

            DeviceContext.VertexShader.SetConstantBuffers(0, _vertexShaderBuffer);
            DeviceContext.PixelShader.SetConstantBuffers(0, _pixelShaderBuffer);

            base.Apply();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct VertexShaderData
        {
            public Matrix3D WorldViewProjection;
            public Matrix3D World;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PixelShaderData
        {
            public Color4F DiffuseColorAndAlpha;
            public Point3D LightPos;
        }
	}
}