using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction
{
    public interface IReportingService
    {
        // Task 5: Get the top 5 best-selling books
        Task<List<(string BookTitle, int TotalSold)>> GetTop5BestSellingBooksAsync();

        // Task 6: Get every customer with the number of purchases they have made
        Task<List<(string CustomerName, int PurchaseCount)>> GetCustomersWithPurchaseCountAsync();

        // Task 7: List categories that contain more than 5 books
        Task<List<(string CategoryName, int BookCount)>> GetCategoriesWithMoreThan5BooksAsync();

        // Task 8: Get every book that costs more than the average book price
        Task<List<Book>> GetBooksAboveAveragePriceAsync();

        // Task 9: Get customers who have never made a purchase
        Task<List<Customer>> GetCustomersWithNoPurchasesAsync();

        // Task 10: Show the total revenue grouped by month
        Task<List<(string MonthYear, decimal TotalRevenue)>> GetTotalRevenueByMonthAsync();
    }
}
