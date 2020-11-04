using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YoYo.Utils
{
    public class StringToIntConverter : JsonConverter<int>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeof(int) == typeToConvert) {
                return true;
            }
            return base.CanConvert(typeToConvert);
        }
        public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                // try to parse number directly from bytes
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
                    return number;

                // try to parse from a string if the above failed, this covers cases with other escaped/UTF characters
                if (int.TryParse(reader.GetString(), out number))
                    return number;
            }

            // fallback to default handling
            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
