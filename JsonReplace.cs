using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PyxeraConcurIntegrationConsole
{
    public class NullReplacementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var jObject = JObject.Load(reader);
                if (jObject.TryGetValue("xsi:nil", StringComparison.OrdinalIgnoreCase, out JToken nilToken) &&
                    nilToken.Value<string>() == "true")
                {
                    // Return an empty string or null
                    return string.Empty;
                }
            }
            return reader.Value?.ToString();
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    public class NullReplacementConverterI : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var jObject = JObject.Load(reader);
                if (jObject.TryGetValue("i:nil", StringComparison.OrdinalIgnoreCase, out JToken nilToken) &&
                    nilToken.Value<string>() == "true")
                {
                    // Return an empty string or null
                    return string.Empty;
                }
            }
            return reader.Value?.ToString();
        }

        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    public class NullReplacementDateConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? ReadJson(JsonReader reader, Type objectType, DateTimeOffset? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var jObject = JObject.Load(reader);
                if (jObject.TryGetValue("xsi:nil", StringComparison.OrdinalIgnoreCase, out JToken nilToken) &&
                    string.Equals(nilToken?.ToString(), "true", StringComparison.OrdinalIgnoreCase))
                {
                    return null; // return null instead of ""
                }
            }

            if (reader.TokenType == JsonToken.String && DateTimeOffset.TryParse(reader.Value?.ToString(), out var dto))
            {
                return dto;
            }

            return null; // fallback
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset? value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                writer.WriteValue(value.Value);
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
    public class SingleOrArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            JToken token = JToken.Load(reader);

            if (token.Type == JTokenType.Array)
            {
                // ✅ Let Newtonsoft handle normal array deserialization
                return token.ToObject<List<T>>(serializer);
            }
            else if (token.Type == JTokenType.Object)
            {
                // ✅ Wrap single object in a list
                return new List<T> { token.ToObject<T>(serializer) };
            }

            // fallback: empty list
            return new List<T>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
