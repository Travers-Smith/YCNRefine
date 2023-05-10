using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Services
{
    public interface IOriginalSourceService
    {
        Task Add(OriginalSource originalSource);

        Task<IEnumerable<Tuple<int, string>>> GetIdAndNamesByDatasetIdWithDataset(int datasetId, int skip, int take);
        
        Task Update(OriginalSource originalSource);
    }
}