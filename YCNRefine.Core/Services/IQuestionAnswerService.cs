using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Services
{
    public interface IQuestionAnswerService
    {
        Task Add(QuestionAnswer questionAnswer);

        Task<IEnumerable<QuestionAnswer>> GetCorrectByOriginalSourceId(int originalSourceId, int skip, int take);

        Task Update(QuestionAnswer questionAnswer);
    }
}