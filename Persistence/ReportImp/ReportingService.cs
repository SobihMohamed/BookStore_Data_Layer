using Application.Abstraction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.ReportImp
{
    public class ReportingService : IReportingService
    {
        private readonly BookDbContext _bookDbContext;
        public ReportingService(BookDbContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        public async Task<List<Book>> GetBooksAboveAveragePriceAsync()
        {
            var averagePrice = _bookDbContext.Books.Average(b => b.Price);
            var booksAboveAverage = await _bookDbContext.Books
                .AsNoTracking()
                .Where(b => b.Price > averagePrice)
                .ToListAsync();
            return booksAboveAverage;
        }

        public async Task<List<(string CategoryName, int BookCount)>> GetCategoriesWithMoreThan5BooksAsync()
        {
            var query = await _bookDbContext.Categories
                .AsNoTracking()
                .Where(c => c.Books.Count > 5)
                .Select(c => new { c.Name, BookCount = c.Books.Count })
                .ToListAsync();

            return query.Select(x => (x.Name, x.BookCount)).ToList();
        }

        public async Task<List<Customer>> GetCustomersWithNoPurchasesAsync()
        {
            var customersWithNoPurchases = await _bookDbContext.Customers
                .AsNoTracking()
                .Where(c => !c.Orders.Any())
                .ToListAsync();
            return customersWithNoPurchases;
        }

        public async Task<List<(string CustomerName, int PurchaseCount)>> GetCustomersWithPurchaseCountAsync()
        {
            var query = await _bookDbContext.Customers
                .AsNoTracking()
                .Select(c => new { c.FullName, PurchaseCount = c.Orders.Count })
                .ToListAsync();

            return query.Select(x => (x.FullName, x.PurchaseCount)).ToList();
        }

        public async Task<List<(string BookTitle, int TotalSold)>> GetTop5BestSellingBooksAsync()
        {
            var query = await _bookDbContext.OrderItems
                .AsNoTracking()
                .GroupBy(oi => oi.Book.Title)
                .Select(g => new { BookTitle = g.Key, TotalSold = g.Sum(oi => oi.Quantity) })
                .OrderByDescending(b => b.TotalSold)
                .Take(5)
                .ToListAsync();
            return query.Select(x => (x.BookTitle, x.TotalSold)).ToList();
        }

        public async Task<List<(string MonthYear, decimal TotalRevenue)>> GetTotalRevenueByMonthAsync()
        {
            var query = await _bookDbContext.Orders
                .AsNoTracking()
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToListAsync();

            return query.Select(x => ($"{x.Year}-{x.Month:D2}", x.TotalRevenue)).ToList();
        }
    }
}
