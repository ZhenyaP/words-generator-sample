using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WordsGeneratorSample.Configuration;
using WordsGeneratorSample.Entities;
using WordsGeneratorSample.Extensions;
using WordsGeneratorSample.Helpers;
using WordsGeneratorSample.Repositories.Interfaces;
using WordsGeneratorSample.Services.Interfaces;

namespace WordsGeneratorSample.Services
{
    public class WordCombinationService : IWordCombinationService
    {
        #region Private

        private readonly WordChunksLengthSubsetSumHelper _chunksLengthSubsetSumHelper;
        private readonly IWordRepository _wordRepository;
        private readonly ConfigSettings _configSettings;
        private readonly ILogger _logger;

        private Dictionary<int, HashSet<string>> GetWordChunksGroupedByLength(IEnumerable<WordChunk> wordChunks)
        {
            var result = wordChunks
                .GroupBy(x => x.Text.Length).
                ToDictionary(gdc => gdc.Key, gdc => gdc
                    .Select(x => x.Text)
                    .ToHashSet());

            return result;
        }

        private async Task<List<WordChunksLengthCombination>> FindAllWordChunksLengthCombinationsAsync(Dictionary<int, HashSet<string>> wordChunksGroupedByLength)
        {
            var cachedCombinations = await _wordRepository.GetWordChunksLengthCombinationsFromCacheAsync();
            if (cachedCombinations?.Count > 0)
                return cachedCombinations;

            var lengths = wordChunksGroupedByLength.Keys
                .ToList();
            var maxLength = lengths.Max();

            var lengthsWithDuplicates = new List<int>(lengths);
            foreach (var length in lengths)
            {
                var duplicatesToAdd = maxLength / length;
                for (var i = 1; i < duplicatesToAdd; i++)
                {
                    lengthsWithDuplicates.Add(length);
                }
            }
            lengthsWithDuplicates.Sort();

            var result = _chunksLengthSubsetSumHelper
                .FindAllWordChunksLengthCombinations(lengthsWithDuplicates)
                .SelectMany(x => x.ChunkLengths.Permute())
                .Select(x => new WordChunksLengthCombination { ChunkLengths = x.ToList() })
                .GroupBy(x => x.ToString())
                .Select(g => g.First())
                .ToList();

            return result;
        }

        private List<string> ParseWordToPieces(string word,
            WordChunksLengthCombination lengthCombination)
        {
            var startIdx = 0;
            var result = new List<string>();
            foreach (var chunkLength in lengthCombination.ChunkLengths)
            {
                result.Add(word.Substring(startIdx, chunkLength));
                startIdx += chunkLength;
            }

            return result;
        }

        private bool ParsedPiecesFoundInChunks(IEnumerable<string> pieces,
            Dictionary<int, HashSet<string>> wordChunksGroupedByLength)
        {
            return pieces
                .All(c => wordChunksGroupedByLength[c.Length]
                    .Contains(c));
        }

        private List<WordCombination> GetWordCombinationsByLengthCombination(
            Dictionary<int, HashSet<string>> wordChunksGroupedByLength,
            List<string> allWords,
            WordChunksLengthCombination lengthCombination)
        {
            var foundCombinations = new ConcurrentBag<WordCombination>();
            allWords
                .ForEach(w =>
                {
                    var pieces = ParseWordToPieces(w, lengthCombination);
                    if (ParsedPiecesFoundInChunks(pieces, wordChunksGroupedByLength))
                    {
                        foundCombinations.Add(new WordCombination
                        {
                            WordChunks = pieces.Select(x => new WordChunk { Text = x }).ToList()
                        });
                    }
                });

            return foundCombinations.ToList();
        }

        #endregion

        #region Public

        public WordCombinationService(
            IWordRepository wordRepository,
            WordChunksLengthSubsetSumHelper chunksLengthSubsetSumHelper,
            IOptions<ConfigSettings> configSettings,
            ILogger<WordCombinationService> logger)
        {
            _wordRepository = wordRepository;
            _chunksLengthSubsetSumHelper = chunksLengthSubsetSumHelper;
            _configSettings = configSettings.Value;
            _logger = logger;
        }

        public async Task<List<WordChunk>> GetAllWordChunksAsync()
        {
            return await _wordRepository.GetAllWordChunksAsync();
        }

        public async Task SaveAllWordCombinationsAsync(List<WordCombination> wordCombinations)
        {
            await _wordRepository.SaveAllWordCombinationsAsync(wordCombinations);
        }

        private List<string> GetAllWordsForCombinationsSearch(Dictionary<int, HashSet<string>> wordChunksGroupedByLength)
        {
            var maxWordLength = _configSettings.WordCombinationGenerationSettings.MaxWordCombinationLength;
            return wordChunksGroupedByLength[maxWordLength].ToList();
        }

        public async Task<List<WordCombination>> GenerateAllWordCombinationsAsync(IEnumerable<WordChunk> wordChunks)
        {
            var start = Stopwatch.GetTimestamp();
            var wordChunksGroupedByLength = GetWordChunksGroupedByLength(wordChunks);
            var elapsedMs = TimeHelper.GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            _logger.LogInformation($"GetWordChunksGroupedByLength - elapsed {elapsedMs:N5} ms.");

            start = Stopwatch.GetTimestamp();
            var wordChunksLengthCombinations = await FindAllWordChunksLengthCombinationsAsync(wordChunksGroupedByLength);
            elapsedMs = TimeHelper.GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            _logger.LogInformation($"FindAllWordChunksLengthCombinations - elapsed {elapsedMs:N5} ms.");

            //TODO: Remove this code (this is used only for development/testing purposes)
            start = Stopwatch.GetTimestamp();
            await _wordRepository.SaveAllLengthsCombinationsAsync(wordChunksLengthCombinations);
            elapsedMs = TimeHelper.GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            _logger.LogInformation($"SaveAllLengthsCombinationsAsync - elapsed {elapsedMs:N5} ms.");

            //TODO END

            start = Stopwatch.GetTimestamp();
            var allWords = GetAllWordsForCombinationsSearch(wordChunksGroupedByLength);
            elapsedMs = TimeHelper.GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            _logger.LogInformation($"GetAllWordsForCombinationsSearch - elapsed {elapsedMs:N5} ms.");

            start = Stopwatch.GetTimestamp();
            var result = wordChunksLengthCombinations
                .SelectMany(x => GetWordCombinationsByLengthCombination(wordChunksGroupedByLength, allWords, x))
                .GroupBy(x => x.ToString())
                .Select(g => g.First())
                .OrderBy(x => x.FullWord)
                .ToList();
            elapsedMs = TimeHelper.GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
            _logger.LogInformation($"GetWordCombinationsByLengthCombination - elapsed {elapsedMs:N5} ms.");

            return result;

        }

        #endregion
    }
}
