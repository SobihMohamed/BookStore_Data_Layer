using Domain.Models;
using Persistence.Context;

namespace Persistence.DataSeed
{
    public static class BookDbContextSeed
    {
        public static async Task SeedAsync(BookDbContext context)
        {
            // 1. Seed Categories & Authors
            if (!context.Categories.Any() && !context.Authors.Any())
            {
                var category1 = new Category { Name = "Software Engineering" };
                var category2 = new Category { Name = "System Design" };

                var author1 = new Author { Name = "Robert C. Martin", Bio = "Author of Clean Code" };
                var author2 = new Author { Name = "Martin Fowler", Bio = "Author of Patterns" };

                await context.Categories.AddRangeAsync(category1, category2);
                await context.Authors.AddRangeAsync(author1, author2);
                await context.SaveChangesAsync();
            }

            // 2. Seed Books (Paperbacks and EBooks)
            if (!context.Books.Any())
            {
                var books = new List<Book>
                {
                    new Paperback { Title = "Clean Code", Price = 150, Stock = 20, AuthorId = 1, CategoryId = 1, ShippingWeight = 1.5m },
                    new Paperback { Title = "Clean Architecture", Price = 250, Stock = 15, AuthorId = 1, CategoryId = 1, ShippingWeight = 1.8m },
                    new EBook { Title = "System Design Interview", Price = 300, Stock = 50, AuthorId = 2, CategoryId = 2, FileSizeMB = 15, DownloadUrl = "link1" },
                    new EBook { Title = "Refactoring", Price = 180, Stock = 30, AuthorId = 2, CategoryId = 1, FileSizeMB = 10, DownloadUrl = "link2" },
                    
                    new Paperback { Title = "Agile Principles", Price = 120, Stock = 10, AuthorId = 1, CategoryId = 1, ShippingWeight = 1.2m },
                    new Paperback { Title = "TDD By Example", Price = 140, Stock = 12, AuthorId = 1, CategoryId = 1, ShippingWeight = 1.1m },
                    new EBook { Title = "Design Patterns", Price = 220, Stock = 40, AuthorId = 2, CategoryId = 1, FileSizeMB = 20, DownloadUrl = "link3" }
                };

                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();
            }

            // 3. Seed Customers
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer { FullName = "Ahmed Ali", Email = "ahmed@test.com", PhoneNumber = "01000000001" },
                    new Customer { FullName = "Sara Hassan", Email = "sara@test.com", PhoneNumber = "01100000002" },
                    new Customer { FullName = "No Purchase Customer", Email = "nopurchase@test.com", PhoneNumber = "01200000003" } // ده عشان Task 9
                };

                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }

            // 4. Seed Orders & OrderItems
            if (!context.Orders.Any())
            {
                var orders = new List<Order>
                {
                    new Order
                    {
                        CustomerId = 1, OrderDate = new DateTime(2026, 5, 10), TotalAmount = 400,
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { BookId = 1, Quantity = 1, UnitPriceAtPurchase = 150 },
                            new OrderItem { BookId = 2, Quantity = 1, UnitPriceAtPurchase = 250 }
                        }
                    },
                    new Order
                    {
                        CustomerId = 2, OrderDate = new DateTime(2026, 6, 15), TotalAmount = 300,
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem { BookId = 3, Quantity = 1, UnitPriceAtPurchase = 300 }
                        }
                    }
                };

                await context.Orders.AddRangeAsync(orders);
                await context.SaveChangesAsync();
            }
        }
    }
}