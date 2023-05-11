# YCNRefine

This is a web application built using .NET 7 and React. 

The application uses Application Insights for logging and SQL Server for the database.

YCNRefine uses OpenAI (https://openai.com/) and Azure OpenAI Service (https://learn.microsoft.com/en-us/azure/cognitive-services/openai/overview) as the two default options but users can add models.

The application uses XUnit/Moq for unit testing the backend.

## Summary

YCNRefine is labelling platform designed to create generative and dialog datasets.

It integrate with GPT and other models to facilitate generative labelling. 

YCN stands for Your Company Name, users should switch it to their company name, e.g. TraversSmithRefine.

The application is built using React, a popular JavaScript library, to create a seamless user experience. 

By plugging into various AI models, the application can provide users with a range of responses, making it a highly versatile tool.

## Getting Started

This application requires a Microsoft SQL Server Database, Azure Key Vault and Application Insights instances to be created. Please see the "Other databases" section for information about using different databases.

Please see sections below for more information on those resources.

To run the application on your local machine, follow these steps:

1.Clone the repository to your local machine.
2. Open the solution file in Visual Studio.
3. Go to the config.json file in the ClientApp/src directory and update "APP_NAME" to your company name.
4. Update the appsettings.json config file.
5. Build the solution.
. In the Package Manager Console, navigate to the YCNRefine.Data project, run the following command to create the database (https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli):
    1.Update - Database(Powershell)
    2.dotnet ef database update (dotnet cli)
7.Start the application by pressing F5 or by clicking the "Play" button in Visual Studio.

## Switching models

To change the chat completion model in YCNRefine, simply go to the appsettings.json file in the main project and update "ChatCompletionType" to the name of your model.

We have provided OpenAI and Azure OpenAI models. 

To select OpenAI update "ChatCompletionType" to "OpenAI", for Azure OpenAI put "AzureOpenAI".

Adding a new model requires 3 steps:

1.Add a new ChatCompletionService in the YCNRefine.Services project and ensure it implements the IChatCompletionService interface.
2.In the ChatModelPickerService add a new case to the GetModel method and give the case the same name as the one provided in the app.
3. Add the new chat completion service to the service collection in the program.cs file in the main project e.g. services.AddTransient<IChatCompletionService, YourNewChatCompletionService>();
4.If the model relies on an external data source, e.g. third party API, add a new repository to the data access layer and add the repository to the UnitOfWork class. 

## Authentication/Authorization

YCNRefine uses OpenIdConnect for authentication and uses Azure Active Directory as the identity management provider.

## Application Insights

The application uses Application Insights for logging. Application Insights is a monitoring service provided by Microsoft that helps you detect and diagnose issues with your web application.

To use Application Insights in your own application, you will need to create an Application Insights instance in the Azure portal and configure your application to use it. 

Once you have done this, you can view telemetry data in the Application Insights portal.

https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview?tabs=net

### Azure KeyVault

The application uses Azure Keyvault for secrets management. The API keys for both Azure OpenAI and OpenAI are stored in Azure Key Vault.

https://learn.microsoft.com/en-us/azure/key-vault/general/overview

## SQL Server

The application uses SQL Server for the database. SQL Server is a relational database management system provided by Microsoft.

To use SQL Server in your own application, you will need to have a SQL Server instance set up and running.

## Other databases

The application has been built with the Unit of Work/Repository design pattern, which allows users to simply change the data access layer (YCNRefine.Data project) when making data persistence changes. Simply change the repositories/unit of work class and the start up configuration in the program.cs file and the application should still work with other data persistence technologies.

## License 

The license is MIT.
