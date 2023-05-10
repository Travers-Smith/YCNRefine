using Microsoft.Extensions.Configuration;
using YCNBot.Data.Repositories;
using YCNRefine.Core;
using YCNRefine.Core.Repositories;
using YCNRefine.Data.Repositories;

namespace YCNRefine.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IChatCompletionRepository? _azureOpenAIChatCompletion;
        private IChatRepository? _chat;
        private IDatasetRepository? _dataset;
        private IGenerativeSampleRepository? _generativeSample;
        private IChatCompletionRepository? _openAIChatCompletion;
        private IOriginalSourceRepository? _originalSource;
        private IQuestionAnswerRepository? _questionAnswer;

        private readonly YcnrefineContext _context;
        private readonly HttpClient _openAIClient;
        private readonly HttpClient _azureOpenAIClient;

        private readonly IConfiguration _configuration;

        public UnitOfWork(YcnrefineContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _azureOpenAIClient = httpClientFactory.CreateClient("AzureOpenAIClient");
            _openAIClient = httpClientFactory.CreateClient("OpenAIClient");
        }

        public IChatCompletionRepository AzureOpenAIChatCompletion => _azureOpenAIChatCompletion ??= new AzureChatCompletionRepository(_azureOpenAIClient, _configuration);

        public IChatRepository Chat => _chat ??= new ChatRepository(_context);

        public IDatasetRepository Dataset => _dataset ??= new DatasetRepository(_context);

        public IGenerativeSampleRepository GenerativeSample => _generativeSample ??= new GenerativeSampleRepository(_context);

        public IChatCompletionRepository OpenAIChatCompletion => _openAIChatCompletion ??= new OpenAIChatCompletionRepository(_openAIClient);

        public IOriginalSourceRepository OriginalSource => _originalSource ??= new OriginalSourceRepository(_context);

        public IQuestionAnswerRepository QuestionAnswer => _questionAnswer ??= new QuestionAnswerRepository(_context);

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
