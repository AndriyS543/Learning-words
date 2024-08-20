using Learning_Words.DBContext;
using Learning_Words.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Repositories.CollectionProviders
{
    public class DatabaseCollectionProvider : ICollectionProviders 
    {
        private readonly VocabularyDbContextFactory _dbContextFactory;

        public DatabaseCollectionProvider(VocabularyDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public Task AddAsync(Collection entity)
        {
            throw new NotImplementedException("Adding collections is not supported.");
        }

        public Task<bool> DeleteAsync(Collection entity)
        {
            throw new NotImplementedException("Deleting collections is not supported.");
        }

        public async Task<IEnumerable<Collection>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Collections.ToListAsync();
            }
        }

        public async Task<Collection> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                Collection? collection = await context.Collections
                  .Where(x => x.Id == id)
                  .Include(x => x.Chapters)
                  .FirstOrDefaultAsync();


                if (collection == null)
                {
                    throw new Exception("Collection not found");
                }

                return collection;
            }
        }
        public Task<bool> UpdateAsync(Collection entity)
        {
            throw new NotImplementedException("Updating collections is not supported.");
        }
    }
}
