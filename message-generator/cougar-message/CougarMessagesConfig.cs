using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net.CougarMessage
{
    public class CougarMessagesConfig : JsonConverter<CougarMessagesConfig>
    {
        public class CougarMessageDestination : JsonConverter<CougarMessageDestination>
        {
            [JsonPropertyName("Root")]
            private string m_strRoot;
            [JsonPropertyName("Messages")]
            private string m_strMessages;
            [JsonPropertyName("Protocol")]
            private string m_strProtocol = "protocol";
            [JsonPropertyName("Include")]
            private string m_strInclude;
            [JsonPropertyName("Helpers")]
            private string m_strHelpers;
            [JsonPropertyName("Components")]
            private string m_strComponents;
            [JsonPropertyName("Consumers")]
            private string m_strConsumers;

            public CougarMessageDestination()
            {
                m_strHelpers = "helpers";
                m_strRoot = "root";
                m_strInclude = "include";
                m_strMessages = "messages";
                m_strComponents = "components";
                m_strConsumers = "consumers";
            }

            public CougarMessageDestination(string strRoot, string strMessages, string strInclude, string strHelpers, string strComponents, string strConsumers)
            {
                m_strHelpers = strHelpers;
                m_strRoot = strRoot;
                m_strInclude = strInclude;
                m_strMessages = strMessages;
                m_strComponents = strComponents;
                m_strConsumers = strConsumers;
            }

            public string Root() => m_strRoot;

            public string MessageDir() => m_strMessages;

            public string IncludeDir() => m_strInclude;

            public string ProtocolDir() => m_strProtocol;

            public string HelpersDir() => m_strHelpers;

            public string ComponentsDir() => m_strComponents;

            public string ConsumersDir() => m_strConsumers;

            public string RootPath(string strFile) => Path.Combine(Root(), strFile);

            public string IncludePath(string strFile) => Path.Combine(Root(), MessageDir(), IncludeDir(), strFile);

            public string IncludeComponentPath(string strFile) => Path.Combine(Root(), MessageDir(), IncludeDir(), ComponentsDir(), strFile);

            public string MessagesPath(string strFile) => Path.Combine(Root(), MessageDir(), strFile);

            public string HelpersPath(string strFile) => Path.Combine(Root(), HelpersDir(), strFile);

            public string ComponentsPath(string strFile) => Path.Combine(Root(), MessageDir(), ComponentsDir(), strFile);

            public string ConsumersPath(string strFile) => Path.Combine(Root(), MessageDir(), ConsumersDir(), strFile);

            public string DotnetPath(string strSubDir) => Path.Combine(Root(), ".net", strSubDir);

            public string DotnetFilePath(string strSubDir, string strFile) => Path.Combine(Root(), ".net", strSubDir, strFile);

            public override CougarMessageDestination Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, CougarMessageDestination value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public string ProtocolPath(string strFile) => Path.Combine(Root(), ProtocolDir(), strFile);
        }

        [JsonPropertyName("SourceRoot")]
        private string m_strSourceRoot = "";
        [JsonPropertyName("Source")]
        private List<string> m_listSourceFiles;
        [JsonPropertyName("ComponentMapping")]
        private string m_strComponents;
        [JsonPropertyName("Destination")]
        private CougarMessageDestination m_msgDestination;
        [JsonPropertyName("MessageLimit")]
        private int m_nMessageLimit;
        [JsonPropertyName("HelperLimit")]
        private int m_nHelperLimit;
        [JsonPropertyName("HelperDir")]
        private string m_strHelperDir;
        [JsonPropertyName("GenerateJava")]
        private bool m_bGenerateJava;
        [JsonPropertyName("PackageName")]
        private string m_strPackageName;
        [JsonPropertyName("AggregateSource")]
        private bool m_bAggregateSource;
        [JsonPropertyName("GenerateTypes")]
        private bool m_bGenerateTypes = true;
        [JsonPropertyName("mongo-metadata")]
        private string m_strMongoMetaDataSrc;
        [JsonPropertyName("generate-targets")]
        private List<CougarGenerationTargetConfig> m_listGenerateTargets;

        private List<TypeMetaData> m_listMetaData;

        public CougarMessagesConfig()
        {
            m_listSourceFiles = new List<string>();
        }

        public CougarMessagesConfig(List<string> listSourceFiles, CougarMessageDestination destinationPath, int nMessageLimit, int nHelperLimit, string strHelperDir, bool bGenerateJava, string strJavaPackage, bool bAggregateSource, bool bGenerateTypes)
        {
            m_listSourceFiles = listSourceFiles;
            m_msgDestination = destinationPath;
            m_nMessageLimit = nMessageLimit;
            m_nHelperLimit = nHelperLimit;
            m_strHelperDir = strHelperDir;
            m_bGenerateJava = bGenerateJava;
            m_strPackageName = strJavaPackage;
            m_bAggregateSource = bAggregateSource;
            m_bGenerateTypes = bGenerateTypes;
        }

        public string GetSourceRoot() => m_strSourceRoot;

        public List<string> GetSourceFiles() => m_listSourceFiles;

        public string GetCougarComponentMapping() => m_strComponents;

        public CougarMessageDestination GetDestination() => m_msgDestination;

        public int GetMessageLimit() => m_nMessageLimit;

        public int GetHelperLimit() => m_nHelperLimit;

        public string GetHelperDirectory() => m_strHelperDir;

        public bool GetGenerateJava() => m_bGenerateJava;

        public string GetPackageName() => m_strPackageName;

        public bool GetAggregateSource() => m_bAggregateSource;

        public bool GetGenerateTypes() => m_bGenerateTypes;

        public List<CougarGenerationTargetConfig> GetGenerateTargets() => m_listGenerateTargets;

        public void SetGenerateTargets(List<CougarGenerationTargetConfig> listGenerateTargets) => m_listGenerateTargets = listGenerateTargets;

        public List<TypeMetaData> GetMongoMetaData()
        {
            if (m_listMetaData != null)
            {
                return m_listMetaData;
            }
            m_listMetaData = new List<TypeMetaData>();
            try
            {
                if (!string.IsNullOrEmpty(m_strMongoMetaDataSrc) && File.Exists(m_strMongoMetaDataSrc))
                {
                    string strJson = File.ReadAllText(m_strMongoMetaDataSrc);
                    m_listMetaData = TypeMetaData.FromJson(strJson);
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
            return m_listMetaData;
        }

        public static CougarMessagesConfig FromJson(string strJson)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new CougarMessagesConfig(), new CougarMessageDestination() }
            };
            var messagesConfig = JsonSerializer.Deserialize<CougarMessagesConfig>(strJson, options);
            messagesConfig.GetMongoMetaData();
            return messagesConfig;
        }

        public override CougarMessagesConfig Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, CougarMessagesConfig value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

