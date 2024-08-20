using Learning_Words.DBContext;
using Learning_Words.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.Repositories.ChapterRroviders
{
    public class DatabaseChapterProvider :  IChapterProviders
    {
        private readonly VocabularyDbContextFactory _dbContextFactory;

        public DatabaseChapterProvider(VocabularyDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<Chapter>> GetAllAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Chapters.ToListAsync();
            }
        }

        public async Task<Chapter> GetByIdAsync(int id)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                Chapter? chapter = await context.Chapters.FindAsync(id);
                if (chapter == null)
                {
                    throw new Exception("Chapter not found");
                }

                return chapter;
            }
        }

        public async Task<IEnumerable<Chapter>> GetChaptersByCollectionAsync(Collection collection)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                return await context.Chapters
                                    .Where(c => c.CollectionId == collection.Id).Include(x=>x.Words)
                                    .ToListAsync();
            }
        }

        public async Task<bool> UpdateAsync(Chapter entity)
        {
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Chapters.Update(entity);
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

        public async Task<bool> DeleteAsync(Chapter entity)
        {
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Chapters.Remove(entity);
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

        public async Task AddAsync(Chapter entity)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                context.Chapters.Add(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
