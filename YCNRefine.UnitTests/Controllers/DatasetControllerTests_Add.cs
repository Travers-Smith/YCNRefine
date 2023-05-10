using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class DatasetControllerTests_Add
    {
        [Fact]
        public async Task AddDataset_Successful_Returns204()
        {
            Mock<IDatasetService> datasetService = new ();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new ();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            string datasetName = "DatasetTest";

            IActionResult result = await new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .AddDataset(new AddDatasetModel
                {
                    Name = datasetName
                });

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            DatasetModel datasetModel = Assert.IsType<DatasetModel>(okObjectResult.Value);

            Assert.Equal(datasetModel.Name, datasetName);

            datasetService.Verify(ds => ds.Add(It.IsAny<Dataset>()), Times.Once);
        }

        [Fact]
        public async Task AddDataset_Unauthorized_ReturnsUnauthorizedResult()
        {
            Mock<IDatasetService> datasetService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            datasetService
                .Setup(cs => cs.Add(It.IsAny<Dataset>())).ThrowsAsync(new Exception());

            IActionResult result = await new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .AddDataset(new AddDatasetModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task AddDataset_Fails_ThrowsException()
        {
            Mock<IDatasetService> datasetService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            datasetService
                .Setup(cs => cs.Add(It.IsAny<Dataset>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new DatasetController(configuration.Object, datasetService.Object, identityService.Object)
                .AddDataset(new AddDatasetModel()));
        }
    }
}
