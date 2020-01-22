using System.Diagnostics;

namespace WordsGeneratorSample.Helpers
{
    public static class TimeHelper
    {
        public static double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double) Stopwatch.Frequency;
        }
    }
}
