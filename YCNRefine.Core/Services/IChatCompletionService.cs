using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Services
{
    public interface IChatCompletionService
    {
        Task<string> AddChatCompletion(IEnumerable<Message> messages, string model);
    }
}