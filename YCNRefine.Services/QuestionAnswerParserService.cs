using YCNRefine.Core.Services;

namespace YCNRefine.Services
{
    public class QuestionAnswerParserService : IQuestionAnswerParserService
    {
        public IEnumerable<Tuple<string, string>> ExtractQuestionAnswers(string message)
        {
            string[] rawQuestionAnswers = message.Split("¬¬¬¬");

            foreach (string rawQuestionAnswer in rawQuestionAnswers)
            {
                if (!string.IsNullOrEmpty(rawQuestionAnswer))
                {
                    IEnumerable<string> currentQuestionAnswers = rawQuestionAnswer.Split("\n");

                    yield return Tuple.Create(currentQuestionAnswers
                        .FirstOrDefault(cqa => cqa.StartsWith("Q")) ?? "", currentQuestionAnswers.FirstOrDefault(cqa => cqa.StartsWith("A")) ?? "");
                }
            }
        }
    }
}
