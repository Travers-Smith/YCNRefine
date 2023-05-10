using YCNRefine.Core;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Services;

namespace YCNRefine.Services
{
    public class QuestionAnswerService : IQuestionAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(QuestionAnswer questionAnswer)
        {
            await _unitOfWork.QuestionAnswer.AddAsync(questionAnswer);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<QuestionAnswer>> GetCorrectByOriginalSourceId(int originalSourceId, int skip, int take)
        {
            return await _unitOfWork.QuestionAnswer.GetCorrectByOriginalSourceId(originalSourceId, skip, take);
        }

        public async Task Update(QuestionAnswer questionAnswer)
        {
            _unitOfWork.QuestionAnswer.Update(questionAnswer);

            await _unitOfWork.CommitAsync();
        }
    }
}
