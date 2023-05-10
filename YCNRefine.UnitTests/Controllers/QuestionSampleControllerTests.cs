using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class QuestionSampleControllerTests
    {
        [Fact]
        public async Task AddQuestionAnswer_Successful_ReturnsOkObjectResult()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IQuestionAnswerService> questionAnswerService = new();

            identityService
                .Setup(identityService => identityService.GetUserIdentifier())
                .Returns(Guid.NewGuid());

            IActionResult result = await new QuestionAnswerController(configuration.Object, identityService.Object, questionAnswerService.Object)
                .Add(new QuestionAnswerModel());

            Assert.IsType<OkObjectResult>(result);
        }
   
        [Fact]
        public async Task AddQuestionAnswer_FailsToAdd_ThrowsException()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IQuestionAnswerService> questionAnswerService = new();

            questionAnswerService
                .Setup(qas => qas.Update(It.IsAny<QuestionAnswer>()))
                .ThrowsAsync(new Exception());

            identityService
                .Setup(identityService => identityService.GetUserIdentifier())
                .Returns(Guid.NewGuid());

            await Assert.ThrowsAsync<Exception>(() => new QuestionAnswerController(configuration.Object, identityService.Object, questionAnswerService.Object)
                .Add(new QuestionAnswerModel()));
        }

        [Fact]
        public async Task AddQuestionAnswer_NoUserId_ReturnsUnauthorized()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IQuestionAnswerService> questionAnswerService = new();

            IActionResult result = await new QuestionAnswerController(configuration.Object, identityService.Object, questionAnswerService.Object)
                .Add(new QuestionAnswerModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetByDatasetId_Success_ReturnsQuestionAnswerModels()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IQuestionAnswerService> questionAnswerService = new();

            int originalSourceId = 1;
            int pageNumber = 1;

            identityService
                .Setup(identityService => identityService.GetUserIdentifier())
                .Returns(Guid.NewGuid());

            questionAnswerService
                .Setup(qas => qas.GetCorrectByOriginalSourceId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new QuestionAnswer[]
                {
                    new QuestionAnswer()
                });

            IActionResult result = await new QuestionAnswerController(configuration.Object, identityService.Object, questionAnswerService.Object)
                .GetByDatasetId(originalSourceId, pageNumber);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<QuestionAnswerModel> questionAnswerModels = Assert.IsAssignableFrom<IEnumerable<QuestionAnswerModel>>(okObjectResult.Value);

            Assert.True(questionAnswerModels.Count() == 1);
        }

        [Fact]
        public async Task GetByDatasetId_Fails_ThrowsException()
        {
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IQuestionAnswerService> questionAnswerService = new();

            int originalSourceId = 1;
            int pageNumber = 1;

            identityService
                .Setup(identityService => identityService.GetUserIdentifier())
                .Returns(Guid.NewGuid());

            questionAnswerService
                .Setup(qas => qas.GetCorrectByOriginalSourceId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new QuestionAnswerController(configuration.Object, identityService.Object, questionAnswerService.Object)
                .GetByDatasetId(originalSourceId, pageNumber));
        }
    }
}

