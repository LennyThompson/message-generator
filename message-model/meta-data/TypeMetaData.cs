using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CougarMessage.Metadata
{
    public class TypeMetaData
    {
        [JsonPropertyName("name")]
        private string m_strName = "";
        
        [JsonPropertyName("add-attribute")]
        private AdditionalAttribute? m_additionalAttr;

        [JsonPropertyName("members")]
        private List<MemberMetadata>? m_listMetaMembers;

        public string Name
        {
            get => m_strName;
            set => m_strName = value;
        }

        public AdditionalAttribute? AdditionalAttr
        {
            get => m_additionalAttr;
            set => m_additionalAttr = value;
        }

        public List<MemberMetadata>? MetaMembers
        {
            get => m_listMetaMembers;
            set => m_listMetaMembers = value;
        }

        public static List<TypeMetaData>? FromJson(JsonSerializerOptions options, string strJson)
        {
            return JsonSerializer.Deserialize<List<TypeMetaData>>(strJson, options);
        }
    }
}