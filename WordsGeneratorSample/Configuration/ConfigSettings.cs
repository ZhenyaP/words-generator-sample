namespace WordsGeneratorSample.Configuration
{
    public class ConfigSettings
    {
        public DataSourceConfigs DataSourceConfigs { get; set; }

        public DataSourceType CurrentDataSourceType { get; set; }

        public WordCombinationGenerationSettings WordCombinationGenerationSettings { get; set; }
    }
}
