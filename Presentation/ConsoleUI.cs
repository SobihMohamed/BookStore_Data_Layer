using Application.Abstraction;
using Domain.Models;

namespace Presentation
{
    public class ConsoleUI
    {
        private readonly IBookService _bookService;
        private readonly IReportingService _reportingService;

        public ConsoleUI(IBookService bookService, IReportingService reportingService)
        {
            _bookService = bookService;
            _reportingService = reportingService;
        }

        public async Task RunAsync()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("              BOOK STORE MANAGEMENT               ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1. List books (Pagination & Search) [Tasks 4,11,12]");
                Console.WriteLine("2. Top 5 best-selling books [Task 5]");
                Console.WriteLine("3. Customers with purchase count [Task 6]");
                Console.WriteLine("4. Categories with > 5 books [Task 7]");
                Console.WriteLine("5. Books above average price [Task 8]");
                Console.WriteLine("6. Customers with NO purchases [Task 9]");
                Console.WriteLine("7. Total revenue by month [Task 10]");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("8. Add a New Book");
                Console.WriteLine("9. Update a Book's Price");
                Console.WriteLine("10. Delete a Book");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==================================================");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter search keyword (or press Enter for all): ");
                        var keyword = Console.ReadLine();
                        var books = await _bookService.GetBooksAsync(keyword, 1, 10);
                        Console.WriteLine($"\n--- Found {books.Count} Books ---");
                        foreach (var b in books)
                            Console.WriteLine($"- [{b.Category?.Name}] {b.Title} by {b.Author?.Name} (Price: {b.Price})");
                        break;

                    case "2":
                        var topBooks = await _reportingService.GetTop5BestSellingBooksAsync();
                        Console.WriteLine("--- Top 5 Best-Selling Books ---");
                        foreach (var item in topBooks) Console.WriteLine($"- {item.BookTitle}: {item.TotalSold} sold");
                        break;

                    case "3":
                        var customerPurchases = await _reportingService.GetCustomersWithPurchaseCountAsync();
                        Console.WriteLine("--- Customers & Purchase Count ---");
                        foreach (var item in customerPurchases) Console.WriteLine($"- {item.CustomerName}: {item.PurchaseCount} purchases");
                        break;

                    case "4":
                        var hugeCategories = await _reportingService.GetCategoriesWithMoreThan5BooksAsync();
                        Console.WriteLine("--- Categories with > 5 Books ---");
                        if (!hugeCategories.Any()) Console.WriteLine("No categories have more than 5 books currently.");
                        foreach (var item in hugeCategories) Console.WriteLine($"- {item.CategoryName}: {item.BookCount} books");
                        break;

                    case "5":
                        var expensiveBooks = await _reportingService.GetBooksAboveAveragePriceAsync();
                        Console.WriteLine("--- Books Above Average Price ---");
                        foreach (var b in expensiveBooks) Console.WriteLine($"- {b.Title} (Price: {b.Price})");
                        break;

                    case "6":
                        var lazyCustomers = await _reportingService.GetCustomersWithNoPurchasesAsync();
                        Console.WriteLine("--- Customers With NO Purchases ---");
                        foreach (var cust in lazyCustomers) Console.WriteLine($"- {cust.FullName} ({cust.Email})");
                        break;

                    case "7":
                        var revenue = await _reportingService.GetTotalRevenueByMonthAsync();
                        Console.WriteLine("--- Total Revenue By Month ---");
                        foreach (var item in revenue) Console.WriteLine($"- Month: {item.MonthYear} | Revenue: {item.TotalRevenue:C}");
                        break;

                    case "8":
                        Console.WriteLine("\n--- Add a New Book ---");
                        try
                        {
                            Console.Write("Enter Book Title: ");
                            string title = Console.ReadLine();

                            Console.Write("Enter Price: ");
                            decimal price = decimal.Parse(Console.ReadLine());

                            Console.Write("Enter Stock Quantity: ");
                            int stock = int.Parse(Console.ReadLine());

                            Console.Write("Enter Author ID: ");
                            int authorId = int.Parse(Console.ReadLine());

                            Console.Write("Enter Category ID: ");
                            int categoryId = int.Parse(Console.ReadLine());

                            Console.Write("Enter Shipping Weight (kg): ");
                            decimal weight = decimal.Parse(Console.ReadLine());

                            var newBook = new Paperback
                            {
                                Title = title,
                                Price = price,
                                Stock = stock,
                                AuthorId = authorId,
                                CategoryId = categoryId,
                                ShippingWeight = weight
                            };

                            await _bookService.AddBookAsync(newBook);
                            Console.WriteLine($"\nSuccess: Added new book '{newBook.Title}' with ID {newBook.Id}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nError during add: {ex.Message}");
                        }
                        break;

                    case "9":
                        Console.WriteLine("\n--- Update a Book's Price ---");
                        try
                        {
                            Console.Write("Enter Book ID to update: ");
                            int updateId = int.Parse(Console.ReadLine());

                            var bookToUpdate = await _bookService.GetBookByIdAsync(updateId);

                            if (bookToUpdate == null)
                            {
                                Console.WriteLine($"Error: No book found with ID {updateId}");
                            }
                            else
                            {
                                Console.WriteLine($"Found Book: '{bookToUpdate.Title}' | Current Price: {bookToUpdate.Price}");
                                Console.Write("Enter new Price: ");
                                decimal newPrice = decimal.Parse(Console.ReadLine());

                                bookToUpdate.Price = newPrice;
                                await _bookService.UpdateBookAsync(bookToUpdate);
                                Console.WriteLine($"Success: Updated book '{bookToUpdate.Title}' price to {newPrice}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nError during update: {ex.Message}");
                        }
                        break;

                    case "10":
                        Console.WriteLine("\n--- Delete a Book ---");
                        try
                        {
                            Console.Write("Enter Book ID to delete: ");
                            int deleteId = int.Parse(Console.ReadLine());

                            var bookToDelete = await _bookService.GetBookByIdAsync(deleteId);

                            if (bookToDelete == null)
                            {
                                Console.WriteLine($"❌ Error: No book found with ID {deleteId}");
                            }
                            else
                            {
                                Console.WriteLine($"Found Book: '{bookToDelete.Title}' by Author ID {bookToDelete.AuthorId}");
                                Console.Write("Are you sure you want to delete this book? (y/n): ");
                                string confirm = Console.ReadLine();

                                if (confirm?.ToLower() == "y")
                                {
                                    await _bookService.DeleteBookAsync(bookToDelete);
                                    Console.WriteLine("Success: Book deleted successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("Deletion cancelled.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nError during delete: {ex.Message}");
                        }
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                }
            }
        }
    }
}