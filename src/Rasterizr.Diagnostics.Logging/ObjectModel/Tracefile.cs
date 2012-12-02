using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Rasterizr.Diagnostics.Logging.Serialization;

namespace Rasterizr.Diagnostics.Logging.ObjectModel
{
	public class Tracefile
	{
		private static readonly JsonSerializerSettings SerializerSettings;

		static Tracefile()
		{
			SerializerSettings = new JsonSerializerSettings
			{
				ConstructorHandling = ConstructorHandling.Default,
				Formatting = Formatting.Indented,
				TypeNameHandling = TypeNameHandling.Auto,
				Converters = new List<JsonConverter>
				{
					new DeviceChildConverter(),
					new DeviceChildArrayConverter(),
					new VertexBufferBindingConverter()
				}
			};
		}

		public List<TracefileFrame> Frames { get; set; }

		public Tracefile()
		{
			Frames = new List<TracefileFrame>();
		}

		public static Tracefile FromTextReader(TextReader textReader)
		{
			return JsonConvert.DeserializeObject<Tracefile>(textReader.ReadToEnd(), SerializerSettings);
		}

		public void Save(TextWriter textWriter)
		{
			textWriter.Write(JsonConvert.SerializeObject(this, SerializerSettings));
		}
	}
}