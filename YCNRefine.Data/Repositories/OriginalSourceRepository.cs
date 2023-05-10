using Microsoft.EntityFrameworkCore;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Repositories;

namespace YCNRefine.Data.Repositories
{
    public class OriginalSourceRepository : Repository<OriginalSource>, IOriginalSourceRepository
    {
        public OriginalSourceRepository(YcnrefineContext context) : base(context) { }

        public async Task<IEnumerable<Tuple<int, string>>> GetIdAndNamesByDatasetIdWithDataset(int datasetId, int skip, int take)
        {
            return await _context.OriginalSources
                .Where(os => os.DatasetId == datasetId)
                .Include(os => os.Dataset)
                .OrderByDescending(os => os.Id)
                .Skip(skip)
                .Take(take)
                .Select(os => Tuple.Create(os.Id, os.Name))
                .ToArrayAsync();
        }
    }
}
