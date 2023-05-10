using Moq;
using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Services;

namespace YCNRefine.UnitTests.Services
{
    public class GenerativeSampleServiceTests
    {
        [Fact]
        public async Task Add_Successful_AddsGenerativeSample()
        {
            Mock<IUnitOfWork> unitOfWork = new ();

            GenerativeSample newGenerativeSample = new();

            unitOfWork.Setup(uow => uow.GenerativeSample.AddAsync(newGenerativeSample));

            await new GenerativeSampleService(unitOfWork.Object).Add(newGenerativeSample);

            unitOfWork.Verify(uow => uow.GenerativeSample.AddAsync(newGenerativeSample), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);    
        }

        [Fact]
        public void Add_Fails_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            GenerativeSample newGenerativeSample = new();

            unitOfWork.Setup(uow => uow.CommitAsync()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => new GenerativeSampleService(unitOfWork.Object).Add(newGenerativeSample));
        }

        [Fact]
        public async Task GetByDataset_Successful_ReturnsGenerativeSample()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            GenerativeSample[] generativeSampleEntities = new GenerativeSample[]
            {
                new GenerativeSample()
            };

            int datasetId = 1;
            int skip = 0;
            int take = 1;

            unitOfWork.Setup(uow => uow.GenerativeSample.GetByDataset(datasetId, skip, take)).ReturnsAsync(generativeSampleEntities);

            IEnumerable<GenerativeSample> generativeSamples = await new GenerativeSampleService(unitOfWork.Object).GetByDataset(datasetId, skip, take);

            Assert.Equal(generativeSampleEntities, generativeSamples);
        }

        [Fact]
        public void GetByIdAndDatasetWithMessages_Fails_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            int datasetId = 1;
            int skip = 0;
            int take = 1;

            unitOfWork.Setup(uow => uow.GenerativeSample.GetByDataset(datasetId, skip, take)).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => new GenerativeSampleService(unitOfWork.Object).GetByDataset(datasetId, skip, take));
        }

        [Fact]
        public async Task Update_Successful_UpdatesGenerativeSample()
        {
            Mock<IUnitOfWork> unitOfWork = new ();

            GenerativeSample newGenerativeSample = new();

            unitOfWork.Setup(uow => uow.GenerativeSample.Update(newGenerativeSample));

            await new GenerativeSampleService(unitOfWork.Object).Update(newGenerativeSample);

            unitOfWork.Verify(uow => uow.GenerativeSample.Update(newGenerativeSample), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public void Update_Fails_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            GenerativeSample newGenerativeSample = new();

            unitOfWork.Setup(uow => uow.CommitAsync()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => new GenerativeSampleService(unitOfWork.Object).Update(newGenerativeSample));
        }
    }
}
