using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CougarMessage.Metadata
{
    public class AdditionalAttribute
    {
        [JsonPropertyName("name")]
        private string m_strName = "";
        
        [JsonPropertyName("params")]
        private List<string> m_listParameters = new List<string>();

        public string Name
        {
            get => m_strName;
            set => m_strName = value;
        }

        public List<string> Parameters
        {
            get => m_listParameters;
            set => m_listParameters = value;
        }

        public void SetParameters(List<string> listParameters)
        {
            m_listParameters = listParameters;
        }
    }
}