﻿using Moq;
using YCNBot.Services;
using YCNRefine.Core;
using YCNRefine.Core.Entities;

namespace YCNRefine.UnitTest.Services
{
    public class OpenAiChatCompletionServiceTests
    {
        [Fact]
        public async Task CompleteChat_Successful_ReturnsMessage()
        {
            var mock = new Mock<IUnitOfWork>();

            string completedMessage = "completed message";

            ChatCompletion chatCompletion = new()
            {
                Choices = new List<ChatCompletionChoice>
                {
                    new ChatCompletionChoice
                    {
                        Message = new ChatCompletionChoiceMessage 
                        { 
                            Content = completedMessage,
                            Role = "System"
                        }
                    }
                }
            };

            mock
                .Setup(x => x.OpenAIChatCompletion.CompleteChat(It.IsAny<AddChatCompletionServiceModel>()))
                .ReturnsAsync(chatCompletion);

            string result = await new OpenAIChatCompletionService(mock.Object).AddChatCompletion(
                new List<Message>
                {
                    new Message
                    {
                        Text = "Test message",
                        IsSystem = false,
                    },
                    new Message
                    {
                        Text = "Test response",
                        IsSystem = true,
                    }
                }, 
                "modelname");

            Assert.Equal(result, completedMessage);
        }

        [Fact]
        public async Task CompleteChat_Fails_ThrowsException()
        {
            var mock = new Mock<IUnitOfWork>();

            mock
                .Setup(x => x.OpenAIChatCompletion.CompleteChat(It.IsAny<AddChatCompletionServiceModel>()))
                .Throws(new Exception());

            await Assert.ThrowsAsync<Exception>(() => new OpenAIChatCompletionService(mock.Object).AddChatCompletion(new List<Message>(), "modelname"));
        }
    }
}
