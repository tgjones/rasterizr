using System.Collections.Generic;
using System.Linq;
using Rasterizr.Diagnostics;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.VertexShader;
using SlimShader;

namespace Rasterizr.Util
{
    internal static class DiagnosticUtilities
    {
        public static void RaiseEvent(object sender, DiagnosticEventHandler @event, params object[] arguments)
        {
            if (@event != null)
                @event(sender, new DiagnosticEventArgs(arguments));
        }

        public static int GetID(DeviceChild deviceChild)
        {
            if (deviceChild == null)
                return -1;
            return deviceChild.ID;
        }

        public static int[] GetIDs(IEnumerable<DeviceChild> deviceChildren)
        {
            if (deviceChildren == null)
                return new int[0];

            return deviceChildren.Select(GetID).ToArray();
        }

        public static void RaisePixelEvent(object sender, PixelEventHandler @event,
            VertexShaderOutput[] vertices, int primitiveID, int x, int y,
            ref Number4 pixelShader, ref Number4 previous, ref Number4 result,
            PixelExclusionReason exclusionReason)
        {
            if (@event != null)
                @event(sender, new PixelEventArgs(vertices, primitiveID, x, y, ref pixelShader, ref previous, ref result, exclusionReason));
        }
    }
}