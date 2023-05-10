using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class OriginalSourceControllerTests
    {
        [Fact]
        public async Task GetByDatasetId_Success_ReturnsOriginalSources()
        {
            Mock<IConfiguration> configuration = new ();
            Mock<IOriginalSourceService> originalSourceServce = new();

            int datasetId = 1;

            originalSourceServce
                .Setup(os => os.GetIdAndNamesByDatasetIdWithDataset(datasetId, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Tuple<int, string>[]
                {
                    Tuple.Create(1, "test")
                });

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(await new OriginalSourceController(configuration.Object, originalSourceServce.Object).GetByDatasetId(datasetId, 1));

            IEnumerable<OriginalSourceModel> originalSourceModels = Assert.IsAssignableFrom<IEnumerable<OriginalSourceModel>>(okObjectResult.Value);

            Assert.True(originalSourceModels.Count() == 1);
        }

        [Fact]
        public async Task GetByDatasetId_Failure_ThrowsException()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IOriginalSourceService> originalSourceServce = new();

            int datasetId = 1;

            originalSourceServce
                .Setup(os => os.GetIdAndNamesByDatasetIdWithDataset(datasetId, It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new OriginalSourceController(configuration.Object, originalSourceServce.Object).GetByDatasetId(1, 1));
        }
    }
}
