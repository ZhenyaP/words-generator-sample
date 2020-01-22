using System.Threading.Tasks;
using WordsGeneratorSample.Services.Interfaces;

namespace WordsGeneratorSample
{
    public class Client
    {
        private readonly IWordCombinationService _wordCombinationService;

        public Client(IWordCombinationService wordCombinationService)
        {
            _wordCombinationService = wordCombinationService;
        }

        public async Task RunAsync()
        {
            var wordChunks = await _wordCombinationService.GetAllWordChunksAsync();
            var wordCombinations = await _wordCombinationService.GenerateAllWordCombinationsAsync(wordChunks);
            await _wordCombinationService.SaveAllWordCombinationsAsync(wordCombinations);
        }
    }
}
