using System.Text.Json.Serialization;

namespace Cougar.Utils
{
    public class TableRule
    {
        [JsonPropertyName("tableName")]
        public string TableName { get; set; }

        [JsonPropertyName("sql")]
        public string SQL { get; set; }
    }
}