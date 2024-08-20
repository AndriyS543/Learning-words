using Learning_Words.DBContext;
using Learning_Words.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Repositories.WordProviders
{
    public class DatabaseWordProvider : IWordProviders 
    {
        private readonly VocabularyDbContextFactory _dbContextFactory;

        public DatabaseWordProvider(VocabularyDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Word entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Words.Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAsync(Word entity)
        {
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Words.Remove(entity);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting entity: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Word>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Words.ToListAsync();
            }
        }
        public async Task<IEnumerable<Word>> GetWordsByChapterAsync(Chapter chapter)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Words
                                    .Where(w => w.ChapterId == chapter.Id)
                                    .ToListAsync();
            }
        }
        public async Task<Word> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                Word? word = await context.Words.FindAsync(id);

                if (word == null)
                {
                    throw new Exception("Word not found");
                }

                return word;
            }
        }

        public async Task<bool> UpdateAsync(Word entity)
        {
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Words.Update(entity);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating entity: {ex.Message}");
                return false;
            }
        }

    }
}
