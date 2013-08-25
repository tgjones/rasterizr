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

        internal static DrawEvent CreateDrawEvent()
        {
            return new DrawEvent
            {
                PrimitiveTopology = PrimitiveTopology.TriangleList,
                Vertices = new []
                {
                    new DrawEventVertex
                    {
                        VertexID = 1470,
                        Data = CreateData()
                    }, 
                    new DrawEventVertex
                    {
                        VertexID = 7736,
                        Data = CreateData()
                    }, 
                    new DrawEventVertex
                    {
                        VertexID = 7735,
                        Data = CreateData()
                    }
                },
                Previous = new Number4(1, 0, 0, 1),
                PixelShader = new Number4(1, 1, 0, 1),
                Result = new Number4(1, 0, 1, 1)
            };
        }

        private static DrawEventVertexData[] CreateData()
        {
            return new[]
            {
                new DrawEventVertexData
                {
                    Semantic = "POSITION",
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