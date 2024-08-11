using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net.CougarMessage.Parser.MessageTypes
{
    public class CougarComponentModel : JsonConverter<CougarGenerateConfig>
    {
        public class ExternalKeyGenerator
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("snippet")]
            public string Snippet { get; set; }
        }

        public class TraceAssociation
        {
            [JsonPropertyName("src")]
            public string Source { get; set; }

            [JsonPropertyName("dest")]
            public string[] Destinations { get; set; }
        }

        public class TimestampFilter
        {
            [JsonPropertyName("range")]
            public bool UseRange { get; set; }

            [JsonPropertyName("suppress")]
            public string SuppressIfFieldExists { get; set; }
        }

        public class ComponentGenerator
        {
            [JsonPropertyName("in")]
            public string InMessage { get; set; }

            [JsonPropertyName("out")]
            public List<string> OutMessages { get; set; }

            [JsonPropertyName("trace")]
            public List<TraceAssociation> TraceMembers { get; set; }

            [JsonPropertyName("timestampFilter")]
            public TimestampFilter TimestampFilter { get; set; }

            [JsonPropertyName("externalKey")]
            public ExternalKeyGenerator ExternalKey { get; set; }
        }

        public class CougarComponent
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("consumer")]
            public List<string> ConsumerMessages { get; set; }

            [JsonPropertyName("generator")]
            public List<ComponentGenerator> GeneratorMessages { get; set; }
        }

        [JsonPropertyName("components")]
        public List<CougarComponent> Components { get; set; }

        public override CougarGenerateConfig Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializationOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, CougarGenerateConfig value, JsonSerializationOptions options)
        {
            throw new NotImplementedException();
        }

        public static CougarComponentModel FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<CougarComponentModel>(json, options);
        }
    }
}

