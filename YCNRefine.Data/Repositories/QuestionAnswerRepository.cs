using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCNRefine.Core.Entities;
using YCNRefine.Core.Repositories;

namespace YCNRefine.Data.Repositories
{
    public class QuestionAnswerRepository : Repository<QuestionAnswer>, IQuestionAnswerRepository
    {
        public QuestionAnswerRepository(YcnrefineContext context) : base(context) { }

        public async Task<IEnumerable<QuestionAnswer>> GetCorrectByOriginalSourceId(int originalSourceId, int skip, int take)
        {
            return await _context.QuestionAnswers
                .Where(qa => qa.OriginalSourceId == originalSourceId && qa.CorrectAnswer)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync();
        }
    }
}
