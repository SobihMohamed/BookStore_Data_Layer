using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Persistence.Context
{
    // this class is used by EF Core tools at design time (Add-Migration) 
    // to create an instance of DbContext because it doesn't run Program.cs
    public class BookDbContextFactory : IDesignTimeDbContextFactory<BookDbContext>
    {
        public BookDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var presentationPath = Path.Combine(basePath, "..", "Presentation");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.Exists(presentationPath) ? presentationPath : basePath)
                .AddJsonFile("appsettings.json", optional: false) // السطر ده كان ناقص
                .Build();

            var builder = new DbContextOptionsBuilder<BookDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new BookDbContext(builder.Options);
        }
    }
}