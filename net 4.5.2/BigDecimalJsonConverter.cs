#if JSON_SERIALIZE
using System.Globalization;
using Newtonsoft.Json;

namespace System.Numerics
{
	[JsonConverter(typeof(BigDecimalJsonConverter))]
	public class BigDecimalJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BigDecimal);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			BigDecimal bigDecimal = (BigDecimal)value;
			writer.WriteValue(bigDecimal.ToString());

			// TODO: Figure out how to write precision.
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
				return null;

			if (reader.TokenType != JsonToken.String)
				throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);

			string value = (string)reader.Value!;

			NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;
			BigDecimalFormatter.TryParse(value, style, CultureInfo.InvariantCulture, out BigDecimal bigDecimal);

			return bigDecimal;

			throw new NotImplementedException();
		}
	}
}
#endif