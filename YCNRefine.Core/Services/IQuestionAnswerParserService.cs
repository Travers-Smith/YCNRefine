namespace YCNRefine.Core.Services
{
    public interface IQuestionAnswerParserService
    {
        IEnumerable<Tuple<string, string>> ExtractQuestionAnswers(string message);
    }
}