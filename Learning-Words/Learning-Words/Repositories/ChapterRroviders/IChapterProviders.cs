using Learning_Words.Models;
using Learning_Words.Repositories;
namespace Learning_Words.Repositories.ChapterRroviders
{
    public interface IChapterProviders : IRepository<Chapter>
    {
        Task<IEnumerable<Chapter>> GetChaptersByCollectionAsync(Collection collection);
    }
}
 