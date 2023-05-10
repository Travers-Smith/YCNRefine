using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class QuestionAnswerGeneratorControllerTests
    {
        [Theory]
        [InlineData("AzureOpenAI")]
        [InlineData("OpenAI")]
        public async Task GenerateFromFreeText_Success_ReturnQuestionAnswerModel(string modelName)
        {
            Mock<IChatModelPickerService> chatModelPickerService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IOriginalSourceService> originalSourceService = new();
            Mock<IQuestionAnswerParserService> questionAnswerParserService = new();
            
            Mock<IChatCompletionService> chatCompletionService = new();

            configuration.SetupGet(c => c["ChatCompletionType"]).Returns(modelName);
            
            identityService.Setup(i => i.GetUserIdentifier()).Returns(Guid.NewGuid());

            chatModelPickerService
                .Setup(cmps => cmps.GetModel(It.IsAny<string>()))
                .Returns(chatCompletionService.Object);

            string modelResponse = "Q: This is a question? A: This is an answer ¬¬¬¬";

            chatCompletionService
                .Setup(ccs => ccs.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ReturnsAsync(modelResponse);

            questionAnswerParserService
                .Setup(qaps => qaps.ExtractQuestionAnswers(modelResponse))
                .Returns(new Tuple<string, string>[]
                {
                    Tuple.Create("This is a question", "This is an answer")
                });


             OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(await new QuestionAnswerGeneratorController(
                chatModelPickerService.Object, 
                configuration.Object, 
                identityService.Object, 
                originalSourceService.Object, 
                questionAnswerParserService.Object)
                .GenerateFromFreeText(new QuestionAnswerGeneratorFreeTextModel
                {
                    DatasetId = 1,
                    OriginalSourceId = 1,
                    SourceText = "This is source test"
                }));

            GeneratedQuestionAnswerModel generatedQuestionAnswerModel = Assert.IsType<GeneratedQuestionAnswerModel>(okObjectResult.Value);

            Assert.Contains(generatedQuestionAnswerModel.QuestionAnswers, qa => qa.Question == "This is a question");

            Assert.Contains(generatedQuestionAnswerModel.QuestionAnswers, qa => qa.Answer == "This is an answer");

            originalSourceService.Verify(oss => oss.Update(It.IsAny<OriginalSource>()), Times.Once);
        }

        [Fact]
        public async Task GenerateFromFreeText_ChatCompletionFailure_ThrowsException()
        {
            Mock<IChatModelPickerService> chatModelPickerService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IOriginalSourceService> originalSourceService = new();
            Mock<IQuestionAnswerParserService> questionAnswerParserService = new();

            Mock<IChatCompletionService> chatCompletionService = new();

            configuration.SetupGet(c => c["ChatCompletionType"]).Returns("OpenAI");

            identityService.Setup(i => i.GetUserIdentifier()).Returns(Guid.NewGuid());

            chatModelPickerService
                .Setup(cmps => cmps.GetModel(It.IsAny<string>()))
                .Returns(chatCompletionService.Object);

            chatCompletionService
                .Setup(ccs => ccs.AddChatCompletion(It.IsAny<IEnumerable<Message>>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new QuestionAnswerGeneratorController(
               chatModelPickerService.Object,
               configuration.Object,
               identityService.Object,
               originalSourceService.Object,
               questionAnswerParserService.Object)
               .GenerateFromFreeText(new QuestionAnswerGeneratorFreeTextModel
               {
                   DatasetId = 1,
                   OriginalSourceId = 1,
                   SourceText = "This is source test"
               }));
        }

        [Fact]
        public async Task GenerateFromFreeText_NoUserId_ReturnsUnauthorized()
        {
            Mock<IChatModelPickerService> chatModelPickerService = new();
            Mock<IConfiguration> configuration = new();
            Mock<IIdentityService> identityService = new();
            Mock<IOriginalSourceService> originalSourceService = new();
            Mock<IQuestionAnswerParserService> questionAnswerParserService = new();

            Assert.IsType<UnauthorizedResult>(await new QuestionAnswerGeneratorController(
                chatModelPickerService.Object,
                configuration.Object,
                identityService.Object,
                originalSourceService.Object,
                questionAnswerParserService.Object)
                .GenerateFromFreeText(new QuestionAnswerGeneratorFreeTextModel()));
        }

        [Fact]
        public async Task GenerateFromFreeText_NoChatCompletionService_ReturnsStatus500()
        {
            Mock<IChatModelPickerService> chatModelPickerService = new ();
            Mock<IConfiguration> configuration = new ();
            Mock<IIdentityService> identityService = new ();
            Mock<IOriginalSourceService> originalSourceService = new ();
            Mock<IQuestionAnswerParserService> questionAnswerParserService = new();

            identityService.Setup(i => i.GetUserIdentifier()).Returns(Guid.NewGuid());

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(await new QuestionAnswerGeneratorController(
                chatModelPickerService.Object,
                configuration.Object,
                identityService.Object,
                originalSourceService.Object,
                questionAnswerParserService.Object)
                .GenerateFromFreeText(new QuestionAnswerGeneratorFreeTextModel
                {
                    SourceText = "This is source test"
                }));

            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}
