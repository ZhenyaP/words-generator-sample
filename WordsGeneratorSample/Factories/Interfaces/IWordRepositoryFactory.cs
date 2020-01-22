using WordsGeneratorSample.Repositories.Interfaces;

namespace WordsGeneratorSample.Factories.Interfaces
{
    public interface IWordRepositoryFactory
    {
        IWordRepository GetRepository();
    }
}
