using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_Words.DBContext
{
    public class VocabularyDbContextFactory
    {
        private readonly string _connectionString;
        public VocabularyDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public VocabularyDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new VocabularyDbContext(options);
        }
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
