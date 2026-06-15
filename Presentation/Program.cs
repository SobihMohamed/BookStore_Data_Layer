using Application.Abstraction;
using Application.ServiceImp;
using Domain.Contracts.UnitOfWorkPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Implementpatterns.UowImplement;
using Persistence.ReportImp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            services.AddDbContext<BookDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IReportingService, ReportingService>();

            services.AddTransient<ConsoleUI>();

            var serviceProvider = services.BuildServiceProvider();

            await using (var scope = serviceProvider.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookDbContext>();

                Console.WriteLine("Checking database connection...");
                if (await dbContext.Database.CanConnectAsync())
                {
                    Console.WriteLine("Success: Connected to SQL Server successfully!");

                    Console.WriteLine("Applying Initial Data Seed...");
                    await Persistence.DataSeed.BookDbContextSeed.SeedAsync(dbContext);
                    Console.WriteLine("Seeding Completed Successfully!\n");

                    var ui = scope.ServiceProvider.GetRequiredService<ConsoleUI>();
                    await ui.RunAsync();
                }
                else
                {
                    Console.WriteLine("Warning: Cannot connect to database.");
                }
            }
        }
    }
}