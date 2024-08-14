using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cougar
{
    public class CougarGenerateConfig : JsonConverter<CougarGenerateConfig>
    {
        [JsonPropertyName("MessageConfig")]
        public CougarMessagesConfig MessageConfig { get; set; }

        [JsonPropertyName("ApiConfig")]
        public CougarApiConfig ApiConfig { get; set; }

        public CougarGenerateConfig()
        {
        }

        public CougarMessagesConfig MessagesConfig()
        {
            return MessageConfig;
        }

        public CougarApiConfig ApiConfig()
        {
            return ApiConfig;
        }

        public static CougarGenerateConfig FromJson(JsonSerializerOptions options, string strJson)
        {
            options.Converters.Add(new CougarGenerateConfig());
            options.Converters.Add(new CougarApiConfig());
            options.Converters.Add(new CougarMessagesConfig());
            return JsonSerializer.Deserialize<CougarGenerateConfig>(strJson, options);
        }

        public override CougarGenerateConfig Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<CougarGenerateConfig>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, CougarGenerateConfig value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        public void SetMessageConfig(CougarMessagesConfig msgConfig)
        {
            MessageConfig = msgConfig;
        }

        public void SetApiConfig(CougarApiConfig apiConfig)
        {
            ApiConfig = apiConfig;
        }
    }
}