using System;
using Newtonsoft.Json;
using SlimShader;

namespace Rasterizr.Diagnostics.Logging.Serialization
{
	public class BytecodeContainerConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("$type");
			writer.WriteValue(value.GetType().AssemblyQualifiedName);
			writer.WritePropertyName("RawBytes");
			writer.WriteValue(((BytecodeContainer) value).RawBytes);
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return BytecodeContainer.Parse(reader.ReadAsBytes());
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BytecodeContainer);
		}
	}
}