using System.Text.Json;
using System.Text.Json.Serialization;

namespace Net.Cougar.Utils
{
    public class TableRules
    {
        [JsonPropertyName("tables")]
        public TableRule[] ListTables { get; set; }

        public TableRule[] TableRules()
        {
            return ListTables;
        }

        public static TableRules FromJson(JsonSerializerOptions options, string jsonString)
        {
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<TableRules>(jsonString, options);
        }
    }
}