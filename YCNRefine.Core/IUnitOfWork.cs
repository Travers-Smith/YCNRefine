using YCNRefine.Core.Repositories;

namespace YCNRefine.Core;

public interface IUnitOfWork : IDisposable
{
    IChatCompletionRepository AzureOpenAIChatCompletion { get; }

    IChatRepository Chat { get; }

    IDatasetRepository Dataset { get; }

    IGenerativeSampleRepository GenerativeSample { get; }

    IChatCompletionRepository OpenAIChatCompletion { get; }

    IOriginalSourceRepository OriginalSource { get; }   

    IQuestionAnswerRepository QuestionAnswer { get; }

    Task CommitAsync();
}
