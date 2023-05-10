using Microsoft.EntityFrameworkCore;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Repositories;

namespace YCNRefine.Data.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(YcnrefineContext context) : base(context) { }

        public async Task<Chat> GetByIdWithMessagesAndDataset(int id)
        {
            return await _context.Chats
                .Where(chat => chat.Id == id)
                .Include(chat => chat.Messages)
                .Include(chat => chat.Dataset)
                .FirstAsync();
        }

        public async Task<IEnumerable<Chat>> GetByDataset(int datasetId)
        {
            return await _context.Chats
                .Where(chat => chat.DatasetId == datasetId)
                .ToArrayAsync();
        }
    }
}
