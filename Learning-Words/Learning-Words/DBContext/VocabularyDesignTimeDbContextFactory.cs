using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.DBContext
{
    public class VocabularyDesignTimeDbContextFactory:IDesignTimeDbContextFactory<VocabularyDbContext>
    {
        public VocabularyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VocabularyDbContext>();
            optionsBuilder.UseSqlite("Data Source=mywords.db");

            return new VocabularyDbContext(optionsBuilder.Options);
        }
    }
}
