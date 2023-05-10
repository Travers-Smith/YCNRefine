using Microsoft.EntityFrameworkCore;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Repositories;

namespace YCNRefine.Data.Repositories
{
    public class GenerativeSampleRepository : Repository<GenerativeSample>, IGenerativeSampleRepository
    {
        public GenerativeSampleRepository(YcnrefineContext context) : base(context) { }

        public async Task<GenerativeSample> GetByIdWithDataset(int id)
        {
            return await _context.GenerativeSamples
                .Where(gs => gs .Id == id)
                .Include(gs =>  gs.Dataset)
                .FirstAsync();
        }

        public async Task<IEnumerable<GenerativeSample>> GetByDataset(int datasetId, int skip, int take)
        {
            return await _context.GenerativeSamples
                .Where(generativeSample => generativeSample.DatasetId == datasetId && !generativeSample.IsDeleted)
                .OrderBy(generativeSample => generativeSample.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }
    }
}
