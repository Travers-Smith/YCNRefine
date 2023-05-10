using Microsoft.EntityFrameworkCore;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Repositories;

namespace YCNRefine.Data.Repositories
{
    public class DatasetRepository : Repository<Dataset>, IDatasetRepository
    {
        public DatasetRepository(YcnrefineContext context) : base(context) { }

        public async Task<IEnumerable<Dataset>> GetDatasets(int skip, int take)
        {
            return await _context.Datasets
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }
    }
}
