using Learning_Words.Models;
using Microsoft.EntityFrameworkCore;
using Learning_Words.DBContext.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.DBContext
{
    public class VocabularyDbContext:DbContext
    {
        public VocabularyDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Word> Words { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

        public DbSet<Collection> Collections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WordConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            modelBuilder.ApplyConfiguration(new CollectionConfiguration());
        }
    }
}
