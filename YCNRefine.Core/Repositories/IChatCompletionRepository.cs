using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Repositories
{
    public interface IChatCompletionRepository
    {
        Task<ChatCompletion> CompleteChat(AddChatCompletionServiceModel addChatCompletion);
    }
}