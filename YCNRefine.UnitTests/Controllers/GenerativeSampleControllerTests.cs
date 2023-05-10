using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class GenerativeSampleControllerTests
    {
        [Fact]
        public async Task Add_Successful_ReturnsOkObjectResult()
        {
            Mock<IGenerativeSampleService> generativeSampleService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            identityService
                .Setup(x => x.GetUserIdentifier())
                .Returns(new Guid());

            IActionResult result = await new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                .Add(new AddGeneratveSampleModel()
                {
                    Input = "This is input",
                    Context = "This is context",
                    Output = "This is output"
                });

            OkObjectResult okOjbectResult = Assert.IsType<OkObjectResult>(result);

            Assert.IsType<GenerativeSampleModel>(okOjbectResult.Value);
        }

        [Fact]
        public async Task Add_Unauthorized_ReturnsUnauthorized()
        {
            Mock<IGenerativeSampleService> generativeSampleService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            IActionResult result = await new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                .Add(new AddGeneratveSampleModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Add_Fails_ThrowsException()
        {
            Mock<IGenerativeSampleService> generativeSampleService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            identityService
                .Setup(x => x.GetUserIdentifier())
                .Returns(new Guid());

            generativeSampleService
                .Setup(x => x.Add(It.IsAny<GenerativeSample>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => 
                new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                    .Add(new AddGeneratveSampleModel()
                    {
                        Input = "This is input",
                        Context = "This is context",
                        Output = "This is output"
                    }));
        }

        [Fact]
        public async Task Delete_Successful_Returns204()
        {
            Mock<IGenerativeSampleService> generativeSampleService = new ();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new ();

            generativeSampleService
                .Setup(x => x.GetByIdWithDataset(1))
                .ReturnsAsync(new GenerativeSample());
  
            IActionResult result = await new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                .Delete(1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, statusCodeResult.StatusCode);

            generativeSampleService.Verify(chatService => chatService.Update(It.IsAny<GenerativeSample>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Fails_ThrowsException()
        {
            Mock<IGenerativeSampleService> generativeSampleService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();

            generativeSampleService
                .Setup(x => x.GetByIdWithDataset(1))
                .ReturnsAsync(new GenerativeSample());

            generativeSampleService
                .Setup(cs => cs.Update(It.IsAny<GenerativeSample>())).ThrowsAsync(new Exception())
                .Verifiable();

            await Assert.ThrowsAsync<Exception>(() => new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                .Delete(1));
        }


        [Fact]
        public async Task GetByDataset_Successful_ReturnsGenerativeSamples()
        {
            IEnumerable<GenerativeSample> datasets = new GenerativeSample[]
            {
                new GenerativeSample(),
            };

            Mock<IConfiguration> configuration = new();
            Mock<IGenerativeSampleService> generativeSampleService = new();
            Mock<IIdentityService> identityService = new();

            generativeSampleService
                .Setup(ds => ds.GetByDataset(1, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(datasets);

            IActionResult result = await new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                .GetByDataset(1, 1);

            OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<GenerativeSampleModel> returnedChats = Assert.IsAssignableFrom<IEnumerable<GenerativeSampleModel>>(actionResult.Value);

            Assert.Equal(datasets.Count(), returnedChats.Count());
        }

        [Fact]
        public async Task GetByDataset_Fails_ThrowsException()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IGenerativeSampleService> generativeSampleService = new();
            Mock<IIdentityService> identityService = new();

            generativeSampleService.Setup(cs => cs.GetByDataset(1, It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new GenerativeSampleController(configuration.Object, generativeSampleService.Object, identityService.Object)
                .GetByDataset(1, 1));
        }

    }
}
