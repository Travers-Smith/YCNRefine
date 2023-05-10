using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Repositories
{
    public interface IGenerativeSampleRepository : IRepository<GenerativeSample>
    {
        Task<GenerativeSample> GetByIdWithDataset(int id);

        Task<IEnumerable<GenerativeSample>> GetByDataset(int datasetId, int skip, int take);
    }
}
