using System;
using Newtonsoft.Json;

namespace Rasterizr.Diagnostics.Logging.Serialization
{
	public class DeviceChildConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(((DeviceChild) value).ID);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return reader.Value;
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(DeviceChild).IsAssignableFrom(objectType);
		}
	}
}