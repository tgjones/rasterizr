using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.InputAssembler;
using Rasterizr.Studio.Modules.GraphicsPixelHistory.ViewModels;
using SlimShader;

namespace Rasterizr.Studio.Modules.GraphicsPixelHistory.Design
{
    public class DesignTimeDrawPixelHistoryEventViewModel : DrawPixelHistoryEventViewModel
    {
        public DesignTimeDrawPixelHistoryEventViewModel()
            : base(CreateDrawEvent())
        {
        }

        internal static DrawEvent CreateDrawEvent(PixelExclusionReason exclusionReason = PixelExclusionReason.NotExcluded)
        {
            return new DrawEvent
            {
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                Vertices = new []
                {
                    new DrawEventVertex
                    {
                        VertexID = 1470,
                        PreVertexShaderData = CreateData(false),
                        PostVertexShaderData = CreateData(true)
                    }, 
                    new DrawEventVertex
                    {
                        VertexID = 7736,
                        PreVertexShaderData = CreateData(false),
                        PostVertexShaderData = CreateData(true)
                    }, 
                    new DrawEventVertex
                    {
                        VertexID = 7735,
                        PreVertexShaderData = CreateData(false),
                        PostVertexShaderData = CreateData(true)
                    }
                },
                Previous = new Number4(1, 0, 0, 1),
                PixelShader = new Number4(1, 1, 0, 1),
                Result = (exclusionReason == PixelExclusionReason.NotExcluded) ? new Number4(1, 0, 1, 1) : (Number4?) null,
                ExclusionReason = exclusionReason
            };
        }

        private static DrawEventVertexData[] CreateData(bool post)
        {
            return new[]
            {
                new DrawEventVertexData
                {
                    Semantic = (post) ? "SV_POSITION" : "POSITION",
                    Value = "x=87.21292, y=-0.3009186, z=8.930054"
                },
                new DrawEventVertexData
                {
                    Semantic = "NORMAL",
                    Value = "x=87.21292, y=-0.3009186, z=8.930054"
                },
                new DrawEventVertexData
                {
                    Semantic = "TEXCOORD",
                    Value = "x=87.21292, y=-0.3009186"
                }
            };
        }
    }
}