﻿using IntranetCore.ConsoleClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Threading.Tasks;

namespace IntranetCore.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // create a new ServiceCollection 
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            // create a new ServiceProvider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // For demo purposes: overall catch-all to log any exception that might 
            // happen to the console & wait for key input afterwards so we can easily 
            // inspect the issue.  
            try
            {
                // Run our IntegrationService containing all samples and
                // await this call to ensure the application doesn't 
                // prematurely exit.
                await serviceProvider.GetService<IIntegrationService>().Run();
            }
            catch (Exception generalException)
            {
                // log the exception
                var logger = serviceProvider.GetService<ILogger<Program>>();
                logger.LogError(generalException,
                    "An exception happened while running the integration service.");
            }

            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add loggers           
            serviceCollection.AddSingleton(new LoggerFactory());
                  //.AddConsole()
                  //.AddDebug());

            serviceCollection.AddLogging();

            // register the integration service on our container with a 
            // scoped lifetime

            // For the CRUD demos
            //serviceCollection.AddScoped<IIntegrationService, CRUDService>();


            serviceCollection.AddScoped<IIntegrationService, PartialUpdateService>();
        }
    }
}
