using System.Collections.Generic;
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

        public static IEnumerable<int> GetIDs(IEnumerable<DeviceChild> deviceChildren)
        {
            if (deviceChildren == null)
                yield break;

            foreach (var deviceChild in deviceChildren)
                yield return GetID(deviceChild);
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