using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using YCNRefine.Controllers;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.UnitTests.Controllers
{
    public class ChatControllerTests
    {
        [Fact]
        public async Task Delete_Successful_Returns204()
        {
            Mock<IChatService> chatService = new ();
            Mock<IIdentityService> identityService = new ();

            chatService.Setup(x => x.GetById(1)).ReturnsAsync(new Chat());

            IActionResult result = await new ChatController(chatService.Object, identityService.Object).Delete(1);

            StatusCodeResult statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal(204, statusCodeResult.StatusCode);

            chatService.Verify(chatService => chatService.Update(It.IsAny<Chat>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Fails_ThrowsException()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            chatService.Setup(x => x.GetById(1)).ReturnsAsync(new Chat());

            chatService
                .Setup(cs => cs.Update(It.IsAny<Chat>())).ThrowsAsync(new Exception())
                .Verifiable();

            await Assert.ThrowsAsync<Exception>(() => new ChatController(chatService.Object, identityService.Object).Delete(1));
        }

        [Fact]
        public async Task GetById_Successful_ReturnsChat()
        {
            Chat chat = new()
            {
                Dataset = new Dataset(),
                Name = "Test",  
                Messages = new Message[]
                {
                    new Message()
                }
            };

            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            int chatId = 1;

            chatService
                .Setup(cs => cs.GetByIdWithMessagesAndDataset(chatId))
                .ReturnsAsync(chat);

            IActionResult result = await new ChatController(chatService.Object, identityService.Object).GetById(chatId);

            OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);

            ChatModel returnedChat = Assert.IsType<ChatModel>(actionResult.Value);

            Assert.Equal(chat.Name, returnedChat.Name);
        }

        [Fact]
        public async Task GetById_Fails_ThrowsException()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            chatService.Setup(cs => cs.GetByIdWithMessagesAndDataset(1)).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(chatService.Object, identityService.Object).GetById(1));

        }

        [Fact]
        public async Task GetByDataset_Successful_ReturnsChats()
        {
            IEnumerable<Chat> chats = new Chat[]
            {
                new Chat(),
            };

            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            int datasetId = 1;

            chatService
                .Setup(cs => cs.GetByDataset(datasetId))
                .ReturnsAsync(chats);

            IActionResult result = await new ChatController(chatService.Object, identityService.Object).GetByDataset(datasetId);

            OkObjectResult actionResult = Assert.IsType<OkObjectResult>(result);

            IEnumerable<ChatModel> returnedChats = Assert.IsAssignableFrom<IEnumerable<ChatModel>>(actionResult.Value);

            Assert.Equal(chats.Count(), returnedChats.Count());
        }

        [Fact]
        public async Task GetByDataset_Fails_ThrowsException()
        {
            Mock<IChatService> chatService = new();
            Mock<IIdentityService> identityService = new();

            chatService.Setup(cs => cs.GetByDataset(1)).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new ChatController(chatService.Object, identityService.Object).GetByDataset(1));
        }
    }
}
