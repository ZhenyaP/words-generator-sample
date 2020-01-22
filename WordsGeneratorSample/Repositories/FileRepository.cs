using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WordsGeneratorSample.Configuration;

namespace WordsGeneratorSample.Repositories
{
    public class FileRepository : BaseRepository<FileDataSourceConfig>
    {
        public FileRepository(FileDataSourceConfig config) : base(config)
        {
        }

        /// <summary>
        /// This is the same default buffer size as
        /// <see cref="StreamReader"/> and <see cref="FileStream"/>.
        /// </summary>
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.
        /// </summary>
        private const FileOptions DefaultOptions = FileOptions.Asynchronous |
                                                   FileOptions.SequentialScan;

        protected Task<string[]> ReadAllLinesAsync(string path)
        {
            return ReadAllLinesAsync(path, Encoding.UTF8);
        }

        protected async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding)
        {
            var lines = new List<string>();

            if (!File.Exists(path))
            {
                return await Task.FromResult(new string[] { });
            }

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize,
                DefaultOptions))
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
                    {
                        lines.Add(line);
                    }
                }
            }

            return lines.ToArray();
        }

        protected async Task WriteTextAsync(string path, string contents)
        {
            await WriteTextAsync(path, contents, Encoding.UTF8);
        }

        protected async Task WriteTextAsync(string path, string contents, Encoding encoding)
        {
            using (var memoryStream = new MemoryStream(encoding.GetBytes(contents)))
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, DefaultBufferSize, DefaultOptions))
                {
                    await memoryStream.CopyToAsync(stream, DefaultBufferSize)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}
