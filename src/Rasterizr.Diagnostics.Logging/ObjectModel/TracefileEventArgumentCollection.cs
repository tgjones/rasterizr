using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class TracefileEventArgumentCollection : Collection<object>
	{
		public TracefileEventArgumentCollection(IList<object> list)
			: base(list)
		{
			
		}

		public TracefileEventArgumentCollection()
		{
			
		}

		public T Get<T>(int index)
		{
			var type = typeof(T);
			if (this[index] == null)
				return default(T);

			if (this[index].GetType() != type)
			{
				if (type.IsEnum)
					type = typeof(int);
				return (T) Convert.ChangeType(this[index], type);
			}
			return (T) this[index];
		}

	    public T Get<T>(Device device, int index)
            where T : DeviceChild
	    {
	        var id = Get<int>(index);
	        return GetDeviceChild<T>(device, id);
	    }

	    public T[] GetArray<T>(Device device, int index)
            where T : DeviceChild
	    {
            var ids = Get<int[]>(index);
            return ids.Select(x => GetDeviceChild<T>(device, x)).ToArray();
	    }

	    private static T GetDeviceChild<T>(Device device, int id)
            where T : DeviceChild
	    {
            if (id == -1)
                return null;
            return device.GetDeviceChild<T>(id);
	    }
	}
}