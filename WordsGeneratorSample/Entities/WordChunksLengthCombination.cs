using System.Collections.Generic;

namespace WordsGeneratorSample.Entities
{
    public class WordChunksLengthCombination
    {
        public List<int> ChunkLengths { get; set; }

        public WordChunksLengthCombination()
        {
            ChunkLengths = new List<int>();
        }

        public override string ToString()
        {
            return string.Join(",", ChunkLengths);
        }
    }
}
