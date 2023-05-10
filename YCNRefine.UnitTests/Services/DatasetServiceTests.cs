using Moq;
using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Services;

namespace YCNRefine.UnitTests.Services;

public class DatasetServiceTests
{
    [Fact]
    public async Task Add_Successful_AddsDataset()
    {
        Mock<IUnitOfWork> unitOfWork = new ();

        Dataset newDataset = new();

        unitOfWork.Setup(uow => uow.Dataset.AddAsync(newDataset));

        await new DatasetService(unitOfWork.Object).Add(newDataset);

        unitOfWork.Verify(uow => uow.Dataset.AddAsync(newDataset), Times.Once);

        unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);    
    }

    [Fact]
    public void Add_Fails_ThrowsException()
    {
        Mock<IUnitOfWork> unitOfWork = new();

        Dataset newDataset = new();

        unitOfWork.Setup(uow => uow.CommitAsync()).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<Exception>(() => new DatasetService(unitOfWork.Object).Add(newDataset));
    }

    [Fact]
    public async Task GetByIdAndDatasetWithMessages_Successful_ReturnsDataset()
    {
        Mock<IUnitOfWork> unitOfWork = new();

        Dataset[] datasetEntities = new Dataset[]
        {
            new Dataset()
        };

        int skip = 0;
        int take = 1;

        unitOfWork.Setup(uow => uow.Dataset.GetDatasets(skip, take)).ReturnsAsync(datasetEntities);

        IEnumerable<Dataset> datasets = await new DatasetService(unitOfWork.Object).GetDatasets(skip, take);

        Assert.Equal(datasetEntities, datasets);
    }

    [Fact]
    public void GetByIdAndDatasetWithMessages_Fails_ThrowsException()
    {
        Mock<IUnitOfWork> unitOfWork = new();

        int skip = 1;
        int take = 2;

        unitOfWork.Setup(uow => uow.Dataset.GetDatasets(skip, take)).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<Exception>(() => new DatasetService(unitOfWork.Object).GetDatasets(skip, take));
    }

    [Fact]
    public async Task Update_Successful_UpdatesDataset()
    {
        Mock<IUnitOfWork> unitOfWork = new ();

        Dataset newDataset = new();

        unitOfWork.Setup(uow => uow.Dataset.Update(newDataset));

        await new DatasetService(unitOfWork.Object).Update(newDataset);

        unitOfWork.Verify(uow => uow.Dataset.Update(newDataset), Times.Once);

        unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public void Update_Fails_ThrowsException()
    {
        Mock<IUnitOfWork> unitOfWork = new();

        Dataset newDataset = new();

        unitOfWork.Setup(uow => uow.CommitAsync()).ThrowsAsync(new Exception());

        Assert.ThrowsAsync<Exception>(() => new DatasetService(unitOfWork.Object).Update(newDataset));
    }
    
}
