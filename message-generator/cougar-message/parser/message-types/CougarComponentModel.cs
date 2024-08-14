using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cougar;

namespace CougarMessage.Parser.MessageTypes
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
        public string? InMessage { get; set; }

        [JsonPropertyName("out")]
        public List<string>? OutMessages { get; set; }

        [JsonPropertyName("trace")]
        public List<TraceAssociation>? TraceMembers { get; set; }

        [JsonPropertyName("timestampFilter")]
        public TimestampFilter? TimestampFilter { get; set; }

        [JsonPropertyName("externalKey")]
        public ExternalKeyGenerator? ExternalKey { get; set; }
    }

    public class CougarComponent
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("consumer")]
        public List<string>? ConsumerMessages { get; set; }

        [JsonPropertyName("generator")]
        public List<ComponentGenerator>? GeneratorMessages { get; set; }
    }
    public class CougarComponentModel
    {

        [JsonPropertyName("components")]
        public List<CougarComponent>? Components { get; set; }

        public static CougarComponentModel? FromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<CougarComponentModel>(json, options);
        }
    }
}

