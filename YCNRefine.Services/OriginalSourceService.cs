using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;

namespace YCNRefine.Services
{
    public class OriginalSourceService : IOriginalSourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OriginalSourceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(OriginalSource originalSource)
        {
            await _unitOfWork.OriginalSource.AddAsync(originalSource);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Tuple<int, string>>> GetIdAndNamesByDatasetIdWithDataset(int datasetId, int skip, int take)
        {
            return await _unitOfWork.OriginalSource.GetIdAndNamesByDatasetIdWithDataset(datasetId, skip, take);
        }

        public async Task Update(OriginalSource originalSource)
        {
            _unitOfWork.OriginalSource.Update(originalSource);

            await _unitOfWork.CommitAsync();
        }
    }
}
