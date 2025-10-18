using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoListApp.DAL.Models;

namespace ToDoList.GUI.Data
{
    public static class DbContextFactory
    {
        private static string? _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .Build();

                    _connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
                }
                return _connectionString;
            }
        }

        public static ToDoListContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ToDoListContext>();
            optionsBuilder.UseSqlServer(ConnectionString, x => x.UseNetTopologySuite());

            return new ToDoListContext(optionsBuilder.Options);
        }
    }
}
