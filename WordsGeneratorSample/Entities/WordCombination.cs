using System.Collections.Generic;

namespace WordsGeneratorSample.Entities
{
    public class WordCombination
    {
        public List<WordChunk> WordChunks { get; set; }

        public string FullWord => string.Concat(WordChunks);

        public WordCombination()
        {
            WordChunks = new List<WordChunk>();
        }

        public override string ToString()
        {
            return $"{string.Join('+', WordChunks)}={FullWord}";
        }
    }
}
