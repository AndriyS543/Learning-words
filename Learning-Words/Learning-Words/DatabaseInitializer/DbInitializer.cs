using Learning_Words.DBContext;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Learning_Words.DatabaseInitializer
{
    public class DbInitializer
    {
        private readonly VocabularyDbContextFactory _dbContextFactory;

        public DbInitializer(VocabularyDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task InitializeAsync()
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                if (!await context.Collections.AnyAsync())
                {
                    // Get the path to the directory with the executable file
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // Move to the root directory of the project
                    var projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));

                    // List of initialization scripts
                    var scripts = new List<string>
                    {
                        Path.Combine(projectRoot, "DatabaseInitializer", "Scripts", "InitializeDatabase.sql"),
            
                        // Add other script paths as needed
                    };

                    using (var connection = new SqliteConnection(_dbContextFactory.GetConnectionString()))
                    {
                        await connection.OpenAsync();
                        using (var transaction = await connection.BeginTransactionAsync())
                        {
                            foreach (var scriptPath in scripts)
                            {
                                if (File.Exists(scriptPath))
                                {
                                    var script = await File.ReadAllTextAsync(scriptPath);
                                    var command = connection.CreateCommand();
                                    command.CommandText = script;
                                    command.Transaction = (SqliteTransaction)transaction;  
                                    await command.ExecuteNonQueryAsync();
                                }
                            }

                            await transaction.CommitAsync();
                        }
                    }
                }
            }
        }
    }
}
