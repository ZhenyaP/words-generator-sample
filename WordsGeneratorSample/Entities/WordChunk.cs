using System.Collections.Generic;

namespace WordsGeneratorSample.Entities
{
    public class WordChunk
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
