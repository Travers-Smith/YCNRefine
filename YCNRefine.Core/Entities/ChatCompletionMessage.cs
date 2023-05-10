using System.Text.Json.Serialization;

namespace YCNRefine.Core.Entities
{
    public class ChatCompletionMessage
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
