using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net.Cougar
{
    public class CougarApiConfig : JsonConverter<CougarApiConfig>
    {
        [JsonPropertyName("Source")]
        public string Source { get; set; }

        [JsonPropertyName("DestinationPath")]
        public string DestinationPath { get; set; }

        [JsonPropertyName("GenerateCpp")]
        public bool? GenerateCpp { get; set; }

        [JsonPropertyName("CppDestinationPath")]
        public string CppDestinationPath { get; set; }

        [JsonPropertyName("JsonSource")]
        public string JsonSource { get; set; }

        [JsonPropertyName("TableRules")]
        public string TableRules { get; set; }

        public CougarApiConfig()
        {
        }

        public CougarApiConfig(string source, string destination, string jsonSource)
        {
            Source = source;
            DestinationPath = destination;
            JsonSource = jsonSource;
        }

        public static CougarApiConfig FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new CougarApiConfig() }
            };
            return JsonSerializer.Deserialize<CougarApiConfig>(json, options);
        }

        public override CougarApiConfig Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<CougarApiConfig>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, CougarApiConfig value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}