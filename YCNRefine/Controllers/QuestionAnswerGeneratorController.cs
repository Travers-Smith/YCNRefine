using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.Controllers
{
    [ApiController]
    [Route("question-answer-generator")]
    public class QuestionAnswerGeneratorController : ControllerBase
    {
        private readonly IChatModelPickerService _chatModelPickerService;
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;
        private readonly IOriginalSourceService _originalSourceService;
        private readonly IQuestionAnswerParserService _questionAnswerParserService;

        public QuestionAnswerGeneratorController(IChatModelPickerService chatModelPickerService, IConfiguration configuration, IIdentityService identityService,
            IOriginalSourceService originalSourceService, IQuestionAnswerParserService questionAnswerParserService)
        {
            _chatModelPickerService = chatModelPickerService;
            _configuration = configuration;
            _identityService = identityService;
            _originalSourceService = originalSourceService;
            _questionAnswerParserService = questionAnswerParserService;
        }

        [HttpPost("generate-from-free-text")]
        public async Task<IActionResult> GenerateFromFreeText(QuestionAnswerGeneratorFreeTextModel data)
        {
            Guid? userId = _identityService.GetUserIdentifier();

            if(userId == null)
            {
                return Unauthorized();
            }

            OriginalSource originalSource = new ()
            {
                Id = data.OriginalSourceId ?? 0,
                Text = data.SourceText,
                DatasetId = data.DatasetId,
                Name = data.SourceText[..Math.Min(data.SourceText.Length, 30)],
                UserIdentifier = userId.Value
            };

            IChatCompletionService? chatCompletionService = _chatModelPickerService.GetModel(_configuration["ChatCompletionType"] ?? "AzureOpenAI");
            
            if(chatCompletionService is null)
            {
                return StatusCode(500);
            }

            string systemMessage = await chatCompletionService
                .AddChatCompletion(new Message[]
                {
                    new Message
                    {
                        IsSystem = false,
                        Text = _configuration["QuestionAnswerPrompt"] ??  
                            "Put this into question answer format with the question being Q: and the answer being A:. " +
                            "At the end of every question and answer put this symbol \"¬¬¬¬\"",
                    },
                    new Message
                    {
                       IsSystem = false,
                       Text = data.SourceText,
                    } 
                }, 
                _configuration["ChatModel"] ?? "");

            await _originalSourceService.Update(originalSource);
           
            return Ok(new GeneratedQuestionAnswerModel
            {
                OriginalSource = new OriginalSourceModel
                {
                    Id = originalSource.Id,
                    Name = originalSource.Name
                },
                QuestionAnswers = _questionAnswerParserService.ExtractQuestionAnswers(systemMessage)
                    .Select(extractedQuestionAnswer => new QuestionAnswerModel
                    {
                        Question = extractedQuestionAnswer.Item1,
                        Answer = extractedQuestionAnswer.Item2
                    })
            });
        }
    }
}
