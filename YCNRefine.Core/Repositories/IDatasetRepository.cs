using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Repositories
{
    public interface IDatasetRepository : IRepository<Dataset>
    {
        Task<IEnumerable<Dataset>> GetDatasets(int take, int skip);
    }
}
