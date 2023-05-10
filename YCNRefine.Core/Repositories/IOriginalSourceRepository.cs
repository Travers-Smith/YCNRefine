using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Repositories
{
    public interface IOriginalSourceRepository : IRepository<OriginalSource>
    {
        Task<IEnumerable<Tuple<int, string>>> GetIdAndNamesByDatasetIdWithDataset(int datasetId, int skip, int take);
    }
}
