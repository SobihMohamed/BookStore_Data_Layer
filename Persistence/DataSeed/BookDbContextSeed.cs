using Domain.Models;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.DataSeed
{
    public static class BookDbContextSeed
    {
        public static async Task SeedAsync(BookDbContext context)
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "DataSeed");

            // 1. Seed Categories
            if (!context.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync(Path.Combine(basePath, "categories.json"));
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories != null && categories.Count > 0)
                {
                    context.Categories.AddRange(categories);
                }
            }

            // 2. Seed Authors
            if (!context.Authors.Any())
            {
                var authorsData = await File.ReadAllTextAsync(Path.Combine(basePath, "authors.json"));
                var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);

                if (authors != null && authors.Count > 0)
                {
                    context.Authors.AddRange(authors);
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}