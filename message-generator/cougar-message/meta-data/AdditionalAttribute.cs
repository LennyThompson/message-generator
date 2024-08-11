using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net.CougarMessage.Metadata
{
    public class AdditionalAttribute : JsonConverter<AdditionalAttribute>
    {
        [JsonPropertyName("name")]
        private string m_strName;
        
        [JsonPropertyName("params")]
        private List<string> m_listParameters = new List<string>();

        public string GetName()
        {
            return m_strName;
        }

        public void SetName(string name)
        {
            m_strName = name;
        }

        public List<string> GetParameters()
        {
            return m_listParameters;
        }

        public void SetParameters(List<string> listParameters)
        {
            m_listParameters = listParameters;
        }

        public override AdditionalAttribute Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement deserialization logic here
            return null;
        }

        public override void Write(Utf8JsonWriter writer, AdditionalAttribute value, JsonSerializerOptions options)
        {
            // Implement serialization logic here
        }
    }
}