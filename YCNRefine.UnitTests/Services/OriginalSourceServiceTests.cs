using Moq;
using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Services;

namespace YCNRefine.UnitTests.Services
{
    public class OriginalSourceServiceTests
    {
        [Fact] 
        public async Task Add_Successful_AddsOriginalSource()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            OriginalSource originalSource = new();

            unitOfWork.Setup(uow => uow.OriginalSource.AddAsync(originalSource));

            await new OriginalSourceService(unitOfWork.Object).Add(originalSource);

            unitOfWork.Verify(uow => uow.OriginalSource.AddAsync(originalSource), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);    
        }

        [Fact]
        public async Task Add_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            OriginalSource originalSource = new();

            unitOfWork
                .Setup(uow => uow.OriginalSource.AddAsync(originalSource))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new OriginalSourceService(unitOfWork.Object).Add(originalSource));

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task GetIdAndNamesByDatasetId_Success_ReturnsIEnumerableOfTuple()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            int originalSourceId = 1;
            int skip = 10;
            int take = 10;

            Tuple<int, string>[] originalSources = new Tuple<int, string>[]
            {
                Tuple.Create(1, "test")
            };

            unitOfWork
                .Setup(uow => uow.OriginalSource.GetIdAndNamesByDatasetIdWithDataset(originalSourceId, skip, take))
                .ReturnsAsync(originalSources);

            Assert.Equal(originalSources, await new OriginalSourceService(unitOfWork.Object).GetIdAndNamesByDatasetIdWithDataset(originalSourceId, skip, take));
        }

        [Fact]
        public async Task GetCorrectByOriginalSourceId_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            int originalSourceId = 1;
            int skip = 10;
            int take = 10;

            OriginalSource[] originalSources = new OriginalSource[]
            {
                new OriginalSource()
            };

            unitOfWork
                .Setup(uow => uow.OriginalSource.GetIdAndNamesByDatasetIdWithDataset(originalSourceId, skip, take))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new OriginalSourceService(unitOfWork.Object).GetIdAndNamesByDatasetIdWithDataset(originalSourceId, skip, take));
        }
        [Fact]
        public async Task Update_Successful_UpdatesOriginalSource()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            OriginalSource originalSource = new();

            unitOfWork.Setup(uow => uow.OriginalSource.Update(originalSource));

            await new OriginalSourceService(unitOfWork.Object).Update(originalSource);

            unitOfWork.Verify(uow => uow.OriginalSource.Update(originalSource), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_Failure_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            OriginalSource originalSource = new();

            unitOfWork
                .Setup(uow => uow.OriginalSource.Update(originalSource))
                .Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new OriginalSourceService(unitOfWork.Object).Update(originalSource));

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}
