using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;

namespace YCNRefine.Services;

public class GenerativeSampleService : IGenerativeSampleService
{
    private readonly IUnitOfWork _unitOfWork;

    public GenerativeSampleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(GenerativeSample generativeSample)
    {
        await _unitOfWork.GenerativeSample.AddAsync(generativeSample);

        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<GenerativeSample>> GetByDataset(int datasetId, int skip, int take)
    {
        return await _unitOfWork.GenerativeSample.GetByDataset(datasetId, skip, take);
    }

    public async Task<GenerativeSample> GetByIdWithDataset(int id)
    {
        return await _unitOfWork.GenerativeSample.GetByIdWithDataset(id);
    }

    public async Task Update(GenerativeSample generativeSample)
    {
        _unitOfWork.GenerativeSample.Update(generativeSample);

        await _unitOfWork.CommitAsync();
    }
}
