using Microsoft.AspNetCore.Mvc;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;
using YCNRefine.Models;

namespace YCNRefine.Controllers
{
    [ApiController]
    [Route("question-answer")]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;
        private readonly IQuestionAnswerService _questionAnswerService;

        public QuestionAnswerController(IConfiguration configuration, IIdentityService identityService, IQuestionAnswerService questionAnswerService)
        {
            _configuration = configuration;
            _identityService = identityService;
            _questionAnswerService = questionAnswerService;
        }

        [HttpPut("add-or-update")]
        public async Task<IActionResult> Add(QuestionAnswerModel questionAnswerData)
        {
            Guid? userId = _identityService.GetUserIdentifier();

            if(userId == null)
            {
                return Unauthorized();
            }

            QuestionAnswer questionAnswer = new ()
            {
                Id = questionAnswerData.Id ?? 0,
                Answer = questionAnswerData.Answer,
                Question = questionAnswerData.Question,
                OriginalSourceId = questionAnswerData.OriginalSourceId,
                UserIdentifier = userId.Value,
                CorrectAnswer = questionAnswerData.CorrectAnswer
            };
                       
            await _questionAnswerService.Update(questionAnswer);
           
            return Ok(questionAnswer.Id);
        }

        [HttpGet("get-by-dataset/{originalSourceCodeId}")]
        public async Task<IActionResult> GetByDatasetId(int originalSourceCodeId, int? pageNumber)
        {
            pageNumber ??= 1;

            int pageSize = int.Parse(_configuration["PageSize"] ?? "100");

            return Ok((await _questionAnswerService
                .GetCorrectByOriginalSourceId(originalSourceCodeId, (pageSize * pageNumber.Value) - pageSize, pageSize))
                .Select(questionAnswer => new QuestionAnswerModel
                {
                    Id = questionAnswer.Id,
                    Question = questionAnswer.Question,
                    Answer = questionAnswer.Answer
                }));
        }
    }
}
