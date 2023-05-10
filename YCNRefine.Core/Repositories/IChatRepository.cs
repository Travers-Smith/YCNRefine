using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat> GetByIdWithMessagesAndDataset(int id);

        Task<IEnumerable<Chat>> GetByDataset(int datasetId);
    }
}
