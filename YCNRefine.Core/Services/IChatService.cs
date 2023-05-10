using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Services
{
    public interface IChatService
    {
        Task Add(Chat chat);

        Task<Chat> GetById(int id);

        Task<Chat> GetByIdWithMessagesAndDataset(int id);

        Task<IEnumerable<Chat>> GetByDataset(int datasetId);

        Task Update(Chat chat);
    }
}