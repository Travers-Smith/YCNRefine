using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;

namespace YCNRefine.Services
{
    public class AzureChatCompletionService : IChatCompletionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AzureChatCompletionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddChatCompletion(IEnumerable<Message> messages, string model)
        {
            ChatCompletion completedChat = await _unitOfWork.AzureOpenAIChatCompletion.CompleteChat(new AddChatCompletionServiceModel
            {
                Messages = messages.Select(x => new ChatCompletionMessage
                {
                    Content = x.Text,
                    Role = x.IsSystem ? "assistant" : "user"
                }),
                Model = model
            });

            return completedChat.Choices
                .Select(x => x.Message.Content)
                .First();
        }
    }
}
