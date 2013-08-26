using System.Collections.Generic;
using System.Linq;

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
    }
}