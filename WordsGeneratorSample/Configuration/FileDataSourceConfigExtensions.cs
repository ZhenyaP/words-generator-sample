using System.IO;
using WordsGeneratorSample.Common;

namespace WordsGeneratorSample.Configuration
{
    public static class FileDataSourceConfigExtensions
    {
        private static string GetFilePath(FileDataSourceConfig config, FileType fileType)
        {
            return Path.Combine(Constants.CurrentAppDirectory,
                Constants.AppDataFolderName,
                fileType == FileType.Input ? config.InputFileName :
                    fileType == FileType.Output ? config.OutputFileName :
                    config.LengthsCombinationsFileName);
        }

        public static string GetInputFilePath(this FileDataSourceConfig config)
        {
            return GetFilePath(config, FileType.Input);
        }

        public static string GetOutputFilePath(this FileDataSourceConfig config)
        {
            return GetFilePath(config, FileType.Output);
        }

        public static string GetLengthsCombinationsFilePath(this FileDataSourceConfig config)
        {
            return GetFilePath(config, FileType.LengthsCombinations);
        }
    }
}
