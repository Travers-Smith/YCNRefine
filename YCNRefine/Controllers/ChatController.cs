using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IIdentityService _identityService;

        public ChatController(IChatService chatService, IIdentityService identityService)
        {
            _chatService = chatService;
            _identityService = identityService;
        }

        [HttpDelete("delete/{chatId}")]
        public async Task<IActionResult> Delete(int chatId)
        {
            Chat chat = await _chatService.GetById(chatId);

            chat.IsDeleted = true;

            await _chatService.Update(chat);

            return StatusCode(204);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Chat chat = await _chatService.GetByIdWithMessagesAndDataset(id);
            
            return Ok(new ChatModel
            {
                DatasetId = chat.DatasetId,
                Dataset = new DatasetModel
                {
                    Id = chat.DatasetId,
                    Name = chat.Dataset.Name
                },
                Messages = chat.Messages.Select(x => new MessageModel
                {
                    Id = x.Id,
                    IsSystem = x.IsSystem,
                    Text = x.Text
                }),
                Name = chat.Name,
            });
        }

        [HttpGet("get-by-dataset/{datasetId}")]
        public async Task<IActionResult> GetByDataset(int datasetId)
        {
            return Ok((await _chatService.GetByDataset(datasetId))
                .Select(chat => new ChatModel
                {
                    Id = chat.Id,
                    DatasetId = chat.DatasetId,
                    Messages = chat.Messages.Select(message => new MessageModel
                    {
                        Id = message.Id,
                        IsSystem = message.IsSystem,
                        Text = message.Text
                    }),
                    Name = chat.Name,
                }));
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveChat(ChatModel saveChat)
        {
            Guid? userIdentifier = _identityService.GetUserIdentifier();

            if(userIdentifier is null)
            {
                return Unauthorized();
            }

            Chat chat; 

            if(saveChat.Id != null)
            {
                chat = await _chatService.GetById(saveChat.Id.Value);
            }
            else
            {
                chat = new ()
                {
                    DatasetId = saveChat.DatasetId,
                    IsDeleted = false,
                    Name = saveChat.Messages
                        ?.Select(message => message.Text[..Math.Min(message.Text.Length, 100)])
                        .FirstOrDefault() ?? "New Chat",
                    UserIdentifier = userIdentifier.Value
                };
            }

            if(saveChat.Messages != null)
            {
                foreach (MessageModel message in saveChat.Messages)
                {
                    chat.Messages.Add(new Message
                    {
                        Id = message.Id ?? 0,
                        IsSystem = message.IsSystem,
                        Text = message.Text
                    });
                }
            }

            await _chatService.Update(chat);

            return Ok(new ChatModel
            {
                Name = chat.Name,
                Id = chat.Id,
                DatasetId = chat.DatasetId
            });
        }
    }
}
