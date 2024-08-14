using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CougarMessage.Metadata
{
    public class TypeMetaData : JsonConverter<TypeMetaData>
    {
        [JsonPropertyName("name")]
        private string m_strName;
        
        [JsonPropertyName("add-attribute")]
        private AdditionalAttribute m_additionalAttr;

        [JsonPropertyName("members")]
        private List<MemberMetadata> m_listMetaMembers;

        public string GetName()
        {
            return m_strName;
        }

        public void SetName(string name)
        {
            m_strName = name;
        }

        public AdditionalAttribute GetAdditionalAttr()
        {
            return m_additionalAttr;
        }

        public void SetAdditionalAttr(AdditionalAttribute additionalAttr)
        {
            m_additionalAttr = additionalAttr;
        }

        public List<MemberMetadata> GetMetaMembers()
        {
            return m_listMetaMembers;
        }

        public void SetMetaMembers(List<MemberMetadata> listMetaMembers)
        {
            m_listMetaMembers = listMetaMembers;
        }

        public static List<TypeMetaData> FromJson(JsonSerializerOptions options, string strJson)
        {
            options.Converters.Add(new TypeMetaData());
            options.Converters.Add(new MemberMetadata());
            options.Converters.Add(new AdditionalAttribute());
            return JsonSerializer.Deserialize<List<TypeMetaData>>(strJson, options);
        }

        public override void Write(Utf8JsonWriter writer, TypeMetaData value, JsonSerializerOptions options)
        {
            // Implement serialization logic here if needed
        }

        public override TypeMetaData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement deserialization logic here if needed
            return null;
        }
    }
}