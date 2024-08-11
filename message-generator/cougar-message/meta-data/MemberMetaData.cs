using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net.CougarMessage.Metadata
{
    public class MemberMetadata : JsonConverter<MemberMetadata>
    {
        [JsonPropertyName("name")]
        private string m_strName;
        [JsonPropertyName("add-attribute")]
        private AdditionalAttribute m_additionalAttr;

        [JsonPropertyName("language")]
        private string m_strLanguage;
        [JsonPropertyName("target-type")]
        private string m_strTargetType;
        [JsonPropertyName("remove-attribute")]
        private string m_strRemoveAttribute;
        [JsonPropertyName("type")]
        private string m_strTypeUpdate;
        [JsonPropertyName("conversion")]
        private string m_strConversion;

        public string GetName()
        {
            return m_strName;
        }

        public void SetName(string strName)
        {
            this.m_strName = strName;
        }

        public AdditionalAttribute GetAdditionalAttr()
        {
            return m_additionalAttr;
        }

        public void SetAdditionalAttr(AdditionalAttribute additionalAttr)
        {
            this.m_additionalAttr = additionalAttr;
        }

        public string GetRemoveAttribute()
        {
            return m_strRemoveAttribute;
        }

        public void SetRemoveAttribute(string strRemoveAttribute)
        {
            this.m_strRemoveAttribute = strRemoveAttribute;
        }

        public string GetTypeUpdate()
        {
            return m_strTypeUpdate;
        }

        public void SetTypeUpdate(string strTypeUpdate)
        {
            this.m_strTypeUpdate = strTypeUpdate;
        }

        public string GetTargetType()
        {
            return m_strTargetType;
        }

        public void SetTargetType(string strTargetType)
        {
            this.m_strTargetType = strTargetType;
        }

        public string GetConversion()
        {
            return m_strConversion;
        }

        public void SetConversion(string strConversion)
        {
            this.m_strConversion = strConversion;
        }

        public bool GetHasConversion()
        {
            return m_strConversion != null && !m_strConversion.Equals(string.Empty);
        }

        public bool GetHasAdditionalAttribute()
        {
            return m_additionalAttr != null;
        }

        public bool GetHasRemoveAttribute()
        {
            return m_strRemoveAttribute != null && !m_strRemoveAttribute.Equals(string.Empty);
        }

        public bool GetHasTypeUpdate()
        {
            return m_strTypeUpdate != null && !m_strTypeUpdate.Equals(string.Empty);
        }

        public bool GetIsCSharp()
        {
            return m_strLanguage.Equals("csharp", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsCPP()
        {
            return m_strLanguage.Equals("cpp", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsJava()
        {
            return m_strLanguage.Equals("java", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsDart()
        {
            return m_strLanguage.Equals("dart", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsJavaScript()
        {
            return m_strLanguage.Equals("js", StringComparison.OrdinalIgnoreCase);
        }

        public override MemberMetadata Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, MemberMetadata value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}


