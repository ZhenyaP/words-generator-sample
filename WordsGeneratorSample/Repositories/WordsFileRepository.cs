using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WordsGeneratorSample.Configuration;
using WordsGeneratorSample.Entities;
using WordsGeneratorSample.Repositories.Interfaces;

namespace WordsGeneratorSample.Repositories
{
    public class WordsFileRepository : FileRepository, IWordRepository
    {
        public WordsFileRepository(FileDataSourceConfig config) : base(config) { }

        public async Task<List<WordChunk>> GetAllWordChunksAsync()
        {
            var result = (await ReadAllLinesAsync(DataSourceConfig.GetInputFilePath()))
                .Select(x => new WordChunk { Text = x })
                .ToList();

            return result;
        }

        public async Task SaveAllLengthsCombinationsAsync(List<WordChunksLengthCombination> wordChunksLengthCombinations)
        {
            //var content = string.Join(Environment.NewLine,
            //    wordChunksLengthCombinations
            //        .Select(x => x.ToString())
            //        .ToList());
            var content = JsonConvert.SerializeObject(wordChunksLengthCombinations);
            await WriteTextAsync(DataSourceConfig.GetLengthsCombinationsFilePath(), content);
        }

        public async Task SaveAllWordCombinationsAsync(List<WordCombination> wordCombinations)
        {
            var content = GetAllWordCombinationsTextReport(wordCombinations);
            await WriteTextAsync(DataSourceConfig.GetOutputFilePath(), content);
        }

        private string GetAllWordCombinationsTextReport(List<WordCombination> wordCombinations)
        {
            var combinationsTexts = wordCombinations
                .GroupBy(x => x.FullWord)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ToString()));

            var builder = new StringBuilder();
            foreach (var fullWord in combinationsTexts.Keys)
            {
                builder.AppendLine();
                builder.AppendLine("-----------------------------------");
                builder.AppendLine(fullWord);
                builder.AppendLine("-----------------------------------");
                foreach (var combinationText in combinationsTexts[fullWord])
                {
                    builder.AppendLine(combinationText);
                }
            }

            return builder.ToString();
        }

        public async Task<List<WordChunksLengthCombination>> GetWordChunksLengthCombinationsFromCacheAsync()
        {
            var content = string.Concat(await ReadAllLinesAsync(DataSourceConfig.GetLengthsCombinationsFilePath()));
            if (string.IsNullOrEmpty(content))
            {
                return await Task.FromResult(new List<WordChunksLengthCombination>());
            }

            var result = JsonConvert.DeserializeObject<List<WordChunksLengthCombination>>(content);

            return result;
        }
    }
}
