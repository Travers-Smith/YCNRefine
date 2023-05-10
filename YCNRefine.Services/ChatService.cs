using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;

namespace YCNRefine.Services;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(Chat chat)
    {
        await _unitOfWork.Chat.AddAsync(chat);

        await _unitOfWork.CommitAsync();
    }

    public async Task<Chat> GetById(int id)
    {
        return await _unitOfWork.Chat.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Chat>> GetByDataset(int datasetId)
    {
        return await _unitOfWork.Chat.GetByDataset(datasetId);
    }

    public async Task<Chat> GetByIdWithMessagesAndDataset(int id)
    {
        return await _unitOfWork.Chat.GetByIdWithMessagesAndDataset(id);
    }

    public async Task Update(Chat chat)
    {
        _unitOfWork.Chat.Update(chat);

        await _unitOfWork.CommitAsync();
    }
}
