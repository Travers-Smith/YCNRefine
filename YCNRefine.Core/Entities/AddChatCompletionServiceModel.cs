using System.Text.Json.Serialization;

namespace YCNRefine.Core.Entities
{
    public class AddChatCompletionServiceModel
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public IEnumerable<ChatCompletionMessage> Messages { get; set; }
    }
}
