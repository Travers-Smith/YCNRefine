using YCNRefine.Core.Entities;

namespace YCNRefine.Core.Repositories
{
    public interface IQuestionAnswerRepository : IRepository<QuestionAnswer>
    {
        Task<IEnumerable<QuestionAnswer>> GetCorrectByOriginalSourceId(int originalSourceId, int skip, int take);
    }
}
