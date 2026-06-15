using Domain.Models;

namespace Application.Abstraction
{
    public interface IBookService
    {
        Task<IReadOnlyList<Book>> GetBooksAsync(string? searchKeyword, int pageIndex, int pageSize);

        // Task 13: العمليات الأساسية
        Task<Book?> GetBookByIdAsync(int id);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Book book);
    }
}
