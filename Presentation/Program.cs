using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;

namespace Presentation
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //Dependency Injection Container
            var services = new ServiceCollection();

            services.AddDbContext<BookDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Service Provider
            var serviceProvider = services.BuildServiceProvider();

            Console.WriteLine("======================================");
            Console.WriteLine("    Book Store Initialized    ");
            Console.WriteLine("======================================\n");
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();

                Console.WriteLine("Checking database connection...");
                if (await dbContext.Database.CanConnectAsync())
                {
                    Console.WriteLine("Success: Connected to SQL Server successfully!");

                    // Seeding
                    Console.WriteLine("Applying Initial Data Seed...");
                    try
                    {
                        await Persistence.DataSeed.BookDbContextSeed.SeedAsync(dbContext);
                        Console.WriteLine("Seeding Completed Successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred during seeding: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Warning: Cannot connect to database yet (Database might not exist, which is normal before running migrations).");
                }
            }
            Console.ReadLine();
        }
    }
}