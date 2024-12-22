using System.Text.Json.Serialization;

namespace KKApi.Models
{
    public class Provider
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Topics { get; set; } = new List<string>();
    }

    public class ProviderTopicsRoot
    {
        [JsonPropertyName("provider_topics")]
        public Dictionary<string, string> ProviderTopics { get; set; } = new Dictionary<string, string>();
    }
}