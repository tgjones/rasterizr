using System;
using Newtonsoft.Json;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Pipeline.InputAssembler;

namespace Rasterizr.Diagnostics.Logging.Serialization
{
	public class VertexBufferBindingConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var binding = (VertexBufferBinding) value;
			serializer.Serialize(writer, new SerializedVertexBufferBinding
			{
				Buffer = binding.Buffer.ID,
				Offset = binding.Offset,
				Stride = binding.Stride
			});
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return new VertexBufferBinding
			{

			};
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(VertexBufferBinding).IsAssignableFrom(objectType);
		}
	}
}