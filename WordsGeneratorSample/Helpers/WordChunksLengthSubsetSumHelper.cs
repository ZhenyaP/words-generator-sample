using System.Collections.Generic;
using System.Linq;
using WordsGeneratorSample.Configuration;
using WordsGeneratorSample.Entities;

namespace WordsGeneratorSample.Helpers
{
    public class WordChunksLengthSubsetSumHelper
    {
        private List<int> _numbers;
        private readonly WordCombinationGenerationSettings _settings;
        private readonly List<WordChunksLengthCombination> _foundCombinations;

        public WordChunksLengthSubsetSumHelper(WordCombinationGenerationSettings settings)
        {
            _settings = settings;
            _foundCombinations = new List<WordChunksLengthCombination>();
        }

        private void SumUpRecursive(List<int> numbers,
            List<int> part)
        {
            var partialSum = part.AsParallel().Sum();

            if (partialSum == _settings.MaxWordCombinationLength &&
                part.Count > 1)
            {
                _foundCombinations.Add(new WordChunksLengthCombination
                {
                    ChunkLengths = part
                });
            }
            if (partialSum >= _settings.MaxWordCombinationLength)
            {
                return;
            }

            for (var i = 0; i < numbers.Count; i++)
            {
                var remaining = new List<int>();

                for (var j = i + 1; j < numbers.Count; j++)
                {
                    remaining.Add(numbers.ElementAt(j));
                }

                var newPart = new List<int>(part) { numbers.ElementAt(i) };
                SumUpRecursive(remaining, newPart);
            }
        }

        private void RecycleFoundCombinations()
        {
            _foundCombinations.Clear();
        }

        public List<WordChunksLengthCombination> FindAllWordChunksLengthCombinations(List<int> numbers)
        {
            RecycleFoundCombinations();
            _numbers = numbers;
            SumUpRecursive(_numbers, new List<int>());
            return _foundCombinations;
        }
    }
}
