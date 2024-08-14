using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CougarMessage
{
    public class CougarGenerationTargetConfig
    {
        [JsonPropertyName("Template")]
        public string Template { get; set; }

        [JsonPropertyName("PathRoot")]
        public string PathRoot { get; set; }

        [JsonPropertyName("Path")]
        public string Path { get; set; }

        [JsonPropertyName("Filename")]
        public string Filename { get; set; }

        [JsonPropertyName("Params")]
        public List<string> Params { get; set; }

        [JsonPropertyName("Generate")]
        public bool Generate { get; set; }

        [JsonPropertyName("ForEach")]
        public string ForEach { get; set; } = "";

        public CougarGenerationTargetConfig()
        {
        }
    }
}