using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YCNRefine.Services;

namespace YCNRefine.UnitTests.Services
{
    public class QuestionAnswerParserServiceTests
    {
        [Fact]
        public void ExtractQuestionAnswers_ContainCorrectSplitSymbols_ReturnsQuestionAnswerIEnumerable()
        {
            IEnumerable<Tuple<string, string>> questionAnswers = new QuestionAnswerParserService()
                .ExtractQuestionAnswers("Q: What is a test? A: This is a test ¬¬¬¬ Q: What is a second test? A: This is a second test ¬¬¬¬");


        }
    }
}
