using System;
using Newtonsoft.Json;
using Rasterizr.Diagnostics.Logging.ObjectModel;

namespace Rasterizr.Diagnostics.Logging.Serialization
{
	public class TracefileEventArgumentCollectionConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var originalTypeNameHandling = serializer.TypeNameHandling;
			serializer.TypeNameHandling = TypeNameHandling.Objects;

			writer.WriteStartArray();
			foreach (var argument in ((TracefileEventArgumentCollection) value))
				serializer.Serialize(writer, argument);
			writer.WriteEndArray();

			serializer.TypeNameHandling = originalTypeNameHandling;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var result = new TracefileEventArgumentCollection();

			reader.Read();
			while (reader.TokenType != JsonToken.EndArray)
			{
				result.Add(serializer.Deserialize(reader));
				reader.Read();
			}

			return result;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(TracefileEventArgumentCollection);
		}
	}
}