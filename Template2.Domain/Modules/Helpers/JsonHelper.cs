using System.Text.Json.Serialization;

namespace Template2.Domain.Modules.Helpers
{
    public static class JsonHelper
    {
        public static string GetJsonPropertyName(Type classType, string entityName)
        {
            var property = classType.GetProperty(entityName);
            if (property == null)
            {
                return string.Empty;
            }

            //// JsonPropertyNameを取得
            var attribute = (JsonPropertyNameAttribute)property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true)[0];
            return attribute.Name;
        }
    }
}
