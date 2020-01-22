using System.Collections.Generic;
using System.Threading.Tasks;
using WordsGeneratorSample.Entities;

namespace WordsGeneratorSample.Services.Interfaces
{
    public interface IWordCombinationService
    {
        Task<List<WordChunk>> GetAllWordChunksAsync();
        Task SaveAllWordCombinationsAsync(List<WordCombination> wordCombinations);
        Task<List<WordCombination>> GenerateAllWordCombinationsAsync(IEnumerable<WordChunk> wordChunks);
    }
}
