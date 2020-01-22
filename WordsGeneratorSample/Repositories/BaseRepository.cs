using WordsGeneratorSample.Configuration.Interfaces;

namespace WordsGeneratorSample.Repositories
{
    public class BaseRepository<T> where T : IDataSourceConfig
    {
        protected T DataSourceConfig { get; set; }

        public BaseRepository(T config)
        {
            DataSourceConfig = config;
        }
    }
}
