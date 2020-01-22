using System.Collections.Generic;
using System.Threading.Tasks;
using WordsGeneratorSample.Entities;

namespace WordsGeneratorSample.Repositories.Interfaces
{
    public interface IWordRepository
    {
        Task<List<WordChunk>> GetAllWordChunksAsync();
        Task SaveAllLengthsCombinationsAsync(List<WordChunksLengthCombination> wordChunksLengthCombinations);
        Task<List<WordChunksLengthCombination>> GetWordChunksLengthCombinationsFromCacheAsync();
        Task SaveAllWordCombinationsAsync(List<WordCombination> wordCombinations);
    }
}
