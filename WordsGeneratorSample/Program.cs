using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using WordsGeneratorSample.Common;
using WordsGeneratorSample.Configuration;
using WordsGeneratorSample.Factories;
using WordsGeneratorSample.Helpers;
using WordsGeneratorSample.Services;
using WordsGeneratorSample.Services.Interfaces;

namespace WordsGeneratorSample
{
    public static class Program
    {
        private static ServiceProvider CreateServiceProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Constants.CurrentAppDirectory)
                .AddJsonFile("appsettings.json", true, true);

            var configuration = builder.Build();

            var services = new ServiceCollection()
                .Configure<ConfigSettings>(configuration.GetSection(nameof(ConfigSettings)))
                .AddOptions();
            var configSettings = configuration
                .GetSection(nameof(ConfigSettings))
                .Get<ConfigSettings>();

            services.AddLogging(LogLevel.Debug);

            return services.AddTransient(p =>
                    new WordRepositoryFactory(configSettings)
                        .GetRepository())
                .AddTransient(p =>
                    new WordChunksLengthSubsetSumHelper(
                        configSettings.WordCombinationGenerationSettings))
                .AddTransient<IWordCombinationService, WordCombinationService>()
                .AddTransient<Client, Client>()
                .BuildServiceProvider();
        }

        private static void AddLogging(this IServiceCollection services, Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            var serilogLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log.txt")
                .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(logLevel);
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
        }


        static async Task Main(string[] args)
        {
            var services = CreateServiceProvider();
            var client = services.GetService<Client>();
            await client.RunAsync();
        }
    }
}
