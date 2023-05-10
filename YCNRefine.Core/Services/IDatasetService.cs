using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Services
{
    public interface IDatasetService
    {
        Task Add(Dataset dataset);

        Task<Dataset> GetById(int id);

        Task<IEnumerable<Dataset>> GetDatasets(int skip, int take);
        
        Task Update(Dataset dataset);
    }
}