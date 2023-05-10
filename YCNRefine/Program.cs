using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Polly;
using System.Security.Cryptography.X509Certificates;
using YCNBot.Services;
using YCNRefine.Core;
using YCNRefine.Core.Services;
using YCNRefine.Data;
using YCNRefine.MessageHandlers;
using YCNRefine.Services;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

AuthorizationPolicy requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
});

string? azureADCertThumbprint = builder.Configuration["AzureADCertThumbprint"];

if (builder.Environment.IsProduction() && azureADCertThumbprint != null)
{
    using X509Store x509Store = new(StoreLocation.CurrentUser);

    x509Store.Open(OpenFlags.ReadOnly);

    X509Certificate2 x509Certificate = x509Store.Certificates
        .Find(
            X509FindType.FindByThumbprint,
            azureADCertThumbprint,
            validOnly: false)
        .OfType<X509Certificate2>()
        .Single();

    string? azureKeyVaultUri = builder.Configuration["AzureKeyVaultUri"];

    if (azureKeyVaultUri != null)
    {
        object value = builder.Configuration.AddAzureKeyVault(
            new Uri(azureKeyVaultUri),
            new ClientCertificateCredential(
                builder.Configuration["AzureAD:TenantId"],
                builder.Configuration["AzureAD:ClientId"],
                x509Certificate));
    }
}

services.AddHttpContextAccessor();

services.AddDbContext<YcnrefineContext>(options => options.UseSqlServer(builder.Configuration
    .GetConnectionString("YcnrefineContext")));

services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddTransient<IChatCompletionService, AzureChatCompletionService>();
services.AddTransient<IChatCompletionService, OpenAIChatCompletionService>();
services.AddTransient<IChatModelPickerService, ChatModelPickerService>();
services.AddTransient<IChatService, ChatService>();
services.AddTransient<IDatasetService, DatasetService>();
services.AddTransient<IGenerativeSampleService, GenerativeSampleService>();
services.AddTransient<IIdentityService, IdentityService>();
services.AddTransient<IOriginalSourceService, OriginalSourceService>();
services.AddTransient<IQuestionAnswerService, QuestionAnswerService>();
services.AddTransient<IQuestionAnswerParserService, QuestionAnswerParserService>();

services.AddScoped<OpenAIClientHandler>();
services.AddScoped<AzureOpenAIClientHandler>();

services.AddHttpClient("OpenAIClient", options =>
{
    string? openAIBaseUrl = builder.Configuration["OpenAIBaseUrl"];

    if (openAIBaseUrl != null)
    {
        options.BaseAddress = new Uri(openAIBaseUrl);
    }
})
   .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder
    .OrResult(x => x.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    .WaitAndRetryAsync(5, retry => TimeSpan.FromMilliseconds(20)))
    .AddHttpMessageHandler<OpenAIClientHandler>();

services.AddHttpClient("AzureOpenAIClient", options =>
{
    string? openAIBaseUrl = builder.Configuration["AzureOpenAIBaseUrl"];

    if (openAIBaseUrl != null)
    {
        options.BaseAddress = new Uri(openAIBaseUrl);
    }
})
   .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder
    .OrResult(x => x.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    .WaitAndRetryAsync(5, retry => TimeSpan.FromMilliseconds(20)))
    .AddHttpMessageHandler<AzureOpenAIClientHandler>();

services
    .AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd");

var app = builder.Build();

app.UsePathBase("/api");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
