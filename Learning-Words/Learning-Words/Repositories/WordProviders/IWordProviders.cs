using Learning_Words.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Repositories.WordProviders
{
    public interface IWordProviders: IRepository<Word>
    {
        Task<IEnumerable<Word>> GetWordsByChapterAsync(Chapter chapter);
    }
}
