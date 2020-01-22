using System;

namespace WordsGeneratorSample.Exceptions
{
    public class DataSourceNotSupportedException : Exception
    {
        public DataSourceNotSupportedException() { }

        public DataSourceNotSupportedException(string message) : base(message) { }

        public DataSourceNotSupportedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
