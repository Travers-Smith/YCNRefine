using Microsoft.AspNetCore.Mvc;
using Moq;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class ChatControllerTests_SaveChat
    {
        [Fact]
        public async Task SaveChat_NewChat_ReturnsOkResult()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            IActionResult result = await new ChatController(chatService.Object, identityService.Object)
                .SaveChat(new ChatModel());

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            Assert.IsType<ChatModel>(okObjectResult.Value);

            chatService.Verify(cs => cs.Update(It.IsAny<Chat>()), Times.Once);
        }

        [Fact]
        public async Task SaveChat_ExistingChat_ReturnsOkResult()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            Chat newChat = new();

            IActionResult result = await new ChatController(chatService.Object, identityService.Object).SaveChat(new ChatModel());

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);

            chatService.Verify(cs => cs.Update(It.IsAny<Chat>()), Times.Once);
        }

        [Fact]
        public async Task SaveChat_Unauthorized_ReturnsUnauthorizedResult()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            IActionResult result = await new ChatController(chatService.Object, identityService.Object).SaveChat(new ChatModel());

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task SaveChat_Fails_ThrowsException()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            identityService.Setup(x => x.GetUserIdentifier()).Returns(new Guid());

            chatService.Setup(cs => cs.Update(It.IsAny<Chat>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(chatService.Object, identityService.Object).SaveChat(new ChatModel()));
        }
    }
}
