using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CougarMessage.Metadata
{
    public class MemberMetadata
    {
        [JsonPropertyName("name")]
        private string m_strName = "";
        [JsonPropertyName("add-attribute")]
        private AdditionalAttribute? m_additionalAttr;

        [JsonPropertyName("language")]
        private string m_strLanguage = "csharp";
        [JsonPropertyName("target-type")]
        private string? m_strTargetType;
        [JsonPropertyName("remove-attribute")]
        private string? m_strRemoveAttribute;
        [JsonPropertyName("type")]
        private string? m_strTypeUpdate;
        [JsonPropertyName("conversion")]
        private string? m_strConversion;
        [JsonPropertyName("serialiser")]
        private string? m_strSerialiser;

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

        public string? RemoveAttribute
        {
            get => m_strRemoveAttribute;
            set => m_strRemoveAttribute = value;
        }

        public string? TypeUpdate
        {
            get => m_strTypeUpdate;
            set => m_strTypeUpdate = value;
        }

        public string? TargetType
        {
            get => m_strTargetType;
            set => m_strTargetType = value;
        }

        public string? Conversion
        {
            get => m_strConversion;
            set => m_strConversion = value;
        }

        public string? Serialiser
        {
            get => m_strSerialiser;
            set => m_strSerialiser = value;
        }

        public bool HasConversion
        {
            get => m_strConversion != null && !m_strConversion.Equals(string.Empty);
        }

        public bool HasAdditionalAttribute
        {
            get => m_additionalAttr != null;
        }

        public bool HasRemoveAttribute
        {
            get => m_strRemoveAttribute != null && !m_strRemoveAttribute.Equals(string.Empty);
        }

        public bool HasTypeUpdate
        {
            get => m_strTypeUpdate != null && !m_strTypeUpdate.Equals(string.Empty);
        }

        public bool HasSerialiser
        {
            get => m_strSerialiser != null && !m_strSerialiser.Equals(string.Empty);
        }

        public bool IsCSharp
        {
            get => m_strLanguage.Equals("csharp", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsCPP
        {
            get => m_strLanguage.Equals("cpp", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsJava
        {
            get => m_strLanguage.Equals("java", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsDart
        {
            get => m_strLanguage.Equals("dart", StringComparison.OrdinalIgnoreCase);
        }

        public bool GetIsJavaScript
        {
            get => m_strLanguage.Equals("js", StringComparison.OrdinalIgnoreCase);
        }
    }
}


