using System;
using System.Linq;
using Newtonsoft.Json;
using Rasterizr.Diagnostics.Logging.ObjectModel;

namespace Rasterizr.Diagnostics.Logging.Serialization
{
	public class DeviceChildArrayConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var originalTypeNameHandling = serializer.TypeNameHandling;
			serializer.TypeNameHandling = TypeNameHandling.Objects;
			serializer.Serialize(writer, new SerializedDeviceChildArray(((DeviceChild[]) value).Select(x => (x != null) ? x.ID : -1).ToArray()));
			serializer.TypeNameHandling = originalTypeNameHandling;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(DeviceChild[]).IsAssignableFrom(objectType);
		}
	}
}