using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Services
{
    public interface IGenerativeSampleService
    {
        Task Add(GenerativeSample generativeSample);

        Task<IEnumerable<GenerativeSample>> GetByDataset(int datasetId, int skip, int take);

        Task<GenerativeSample> GetByIdWithDataset(int id);

        Task Update(GenerativeSample generativeSample);
    }
}