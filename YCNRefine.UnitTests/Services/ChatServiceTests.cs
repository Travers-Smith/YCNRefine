using Moq;
using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Services;

namespace YCNRefine.UnitTests.Services
{
    public class ChatServiceTests
    {
        [Fact]
        public async Task Add_Successful_AddsChat()
        {
            Mock<IUnitOfWork> unitOfWork = new ();

            Chat newChat = new();

            unitOfWork.Setup(uow => uow.Chat.AddAsync(newChat));

            await new ChatService(unitOfWork.Object).Add(newChat);

            unitOfWork.Verify(uow => uow.Chat.AddAsync(newChat), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);    
        }

        [Fact]
        public void Add_Fails_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            Chat newChat = new();

            unitOfWork.Setup(uow => uow.CommitAsync()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => new ChatService(unitOfWork.Object).Add(newChat));
        }

        [Fact]
        public async Task GetByIdAndDatasetWithMessages_Successful_ReturnsChat()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            Chat chatEntity = new();

            int chatId = 2;

            unitOfWork.Setup(uow => uow.Chat.GetByIdWithMessagesAndDataset(chatId)).ReturnsAsync(chatEntity);

            Chat chat = await new ChatService(unitOfWork.Object).GetByIdWithMessagesAndDataset(chatId);

            Assert.Equal(chatEntity, chat);
        }

        [Fact]
        public void GetByIdAndDatasetWithMessages_Fails_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            int chatId = 2;

            unitOfWork.Setup(uow => uow.Chat.GetByIdWithMessagesAndDataset(chatId)).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => new ChatService(unitOfWork.Object).GetByIdWithMessagesAndDataset(chatId));
        }

        [Fact]
        public async Task Update_Successful_UpdatesChat()
        {
            Mock<IUnitOfWork> unitOfWork = new ();

            Chat newChat = new();

            unitOfWork.Setup(uow => uow.Chat.Update(newChat));

            await new ChatService(unitOfWork.Object).Update(newChat);

            unitOfWork.Verify(uow => uow.Chat.Update(newChat), Times.Once);

            unitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public void Update_Fails_ThrowsException()
        {
            Mock<IUnitOfWork> unitOfWork = new();

            Chat newChat = new();

            unitOfWork.Setup(uow => uow.CommitAsync()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => new ChatService(unitOfWork.Object).Update(newChat));
        }
    }
}
