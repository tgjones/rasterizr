using System.Collections.Generic;
using Rasterizr.Diagnostics;

namespace Rasterizr
{
	public class Device
	{
		private readonly Dictionary<int, DeviceChild> _deviceChildMap;
		private readonly GraphicsLoggerCollection _loggers;
		private readonly DeviceContext _immediateContext;
		private int _id;

		internal GraphicsLoggerCollection Loggers
		{
			get { return _loggers; }
		}

		public DeviceContext ImmediateContext
		{
			get { return _immediateContext; }
		}

		public Device(params GraphicsLogger[] loggers)
		{
			_deviceChildMap = new Dictionary<int, DeviceChild>();
			if (loggers == null)
				loggers = new GraphicsLogger[0];
			_loggers = new GraphicsLoggerCollection(loggers);
			_immediateContext = new DeviceContext(this);
			_loggers.BeginOperation(OperationType.DeviceCreate);
		}

		internal T GetDeviceChild<T>(int id)
			where T : DeviceChild
		{
			return (T) _deviceChildMap[id];
		}

		internal int RegisterDeviceChild(DeviceChild deviceChild)
		{
			int id = _id;
			_deviceChildMap[id] = deviceChild;
			_id++;
			return id;
		}
	}
}