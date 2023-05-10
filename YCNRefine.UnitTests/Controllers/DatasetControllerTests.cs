using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class DatasetControllerTests
    {
        [Fact]
        public async Task Delete_Successful_Returns204()
        {
            Mock<IDatasetService> datasetService = new ();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new ();

            datasetService.Setup(ds => ds.GetById(1)).ReturnsAsync(new Dataset());

            IActionResult result = await new DatasetController(configuration.Object, datasetService.Object, identityService.Object).Delete(1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, statusCodeResult.StatusCode);

            datasetService.Verify(chatService => chatService.Update(It.IsAny<Dataset>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Fails_ThrowsException()
        {
            Mock<IDatasetService> datasetService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            datasetService.Setup(ds => ds.GetById(1)).ReturnsAsync(new Dataset());

            datasetService
                .Setup(cs => cs.Update(It.IsAny<Dataset>())).ThrowsAsync(new Exception())
                .Verifiable();

            await Assert.ThrowsAsync<Exception>(() => new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .Delete(1));
        }


        [Fact]
        public async Task GetDatasets_Successful_ReturnsDatasets()
        {
            IEnumerable<Dataset> datasets = new Dataset[]
            {
                new Dataset(),
            };

            Mock<IConfiguration> configuration = new();
            Mock<IDatasetService> datasetService = new();
            Mock<IIdentityService> identityService = new();

            datasetService
                .Setup(ds => ds.GetDatasets(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(datasets);

            IActionResult result = await new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .GetDatasets(1);

            OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<DatasetModel> returnedDatasets = Assert.IsAssignableFrom<IEnumerable<DatasetModel>>(actionResult.Value);

            Assert.Equal(datasets.Count(), returnedDatasets.Count());
        }

        [Fact]
        public async Task GetByDataset_Fails_ThrowsException()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IDatasetService> datasetService = new();
            Mock<IIdentityService> identityService = new();

            datasetService.Setup(cs => cs.GetDatasets(It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .GetDatasets(1));
        }

        [Fact]
        public async Task UpdateDataset_Successful_Returns204()
        {
            Mock<IDatasetService> datasetService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            datasetService.Setup(ds => ds.GetById(1)).ReturnsAsync(new Dataset());

            IActionResult result = await new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .UpdateDataset(new DatasetModel
                {
                    Id = 1
                });

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, statusCodeResult.StatusCode);

            datasetService.Verify(chatService => chatService.Update(It.IsAny<Dataset>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDataset_Fails_ThrowsException()
        {
            Mock<IDatasetService> datasetService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            datasetService.Setup(ds => ds.GetById(1)).ReturnsAsync(new Dataset());

            datasetService
                .Setup(cs => cs.Update(It.IsAny<Dataset>())).ThrowsAsync(new Exception())
                .Verifiable();

            await Assert.ThrowsAsync<Exception>(() => new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .UpdateDataset(new DatasetModel
                {
                    Id = 1
                }));
        }
    }
}
