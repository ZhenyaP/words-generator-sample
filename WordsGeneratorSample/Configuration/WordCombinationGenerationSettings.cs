using Newtonsoft.Json;

namespace WordsGeneratorSample.Configuration
{
    //[JsonObject("WordCombinationGenerationSettings")]
    public class WordCombinationGenerationSettings
    {
        //[JsonProperty("MaxWordCombinationLength")]
        public int MaxWordCombinationLength { get; set; }

        //TODO: Think if this setting really needed
        //public int WordChunksPerCombinationCount { get; set; }
    }
}
