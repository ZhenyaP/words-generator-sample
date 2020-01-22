using WordsGeneratorSample.Configuration.Interfaces;

namespace WordsGeneratorSample.Configuration
{
    public class FileDataSourceConfig : IDataSourceConfig
    {
        public string InputFileName { get; set; }

        public string OutputFileName { get; set; }

        public string LengthsCombinationsFileName { get; set; }
    }
}
