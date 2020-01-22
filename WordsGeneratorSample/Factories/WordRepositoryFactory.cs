using Microsoft.Extensions.Options;
using WordsGeneratorSample.Configuration;
using WordsGeneratorSample.Exceptions;
using WordsGeneratorSample.Factories.Interfaces;
using WordsGeneratorSample.Repositories;
using WordsGeneratorSample.Repositories.Interfaces;

namespace WordsGeneratorSample.Factories
{
    public class WordRepositoryFactory : IWordRepositoryFactory
    {
        private readonly ConfigSettings _configSettings;

        public WordRepositoryFactory(ConfigSettings configSettings)
        {
            _configSettings = configSettings;
        }


        public IWordRepository GetRepository()
        {
            switch (_configSettings.CurrentDataSourceType)
            {
                case DataSourceType.File:
                    return new WordsFileRepository(_configSettings.DataSourceConfigs.FileDataSourceConfig);
                default:
                    throw new DataSourceNotSupportedException(
                        $"Data source with type {_configSettings.CurrentDataSourceType} is currently not supported!");
            }
        }
    }
}
