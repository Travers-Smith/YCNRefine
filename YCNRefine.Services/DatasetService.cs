using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;

namespace YCNRefine.Services;

public class DatasetService : IDatasetService
{
    private readonly IUnitOfWork _unitOfWork;

    public DatasetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Add(Dataset dataset)
    {
        await _unitOfWork.Dataset.AddAsync(dataset);

        await _unitOfWork.CommitAsync();
    }

    public async Task<Dataset> GetById(int id)
    {
        return await _unitOfWork.Dataset.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Dataset>> GetDatasets(int skip, int take)
    {
        return await _unitOfWork.Dataset.GetDatasets(skip, take);
    }

    public async Task Update(Dataset dataset)
    {
        _unitOfWork.Dataset.Update(dataset);

        await _unitOfWork.CommitAsync();
    }
}
