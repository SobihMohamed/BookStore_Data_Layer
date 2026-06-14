using Application.Specifications;
using Domain.Abstraction;
using Domain.Contracts.UnitOfWorkPattern;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ServiceImp
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Book>> GetBooksAsync(string? searchKeyword, int pageIndex, int pageSize)
        {
            var spec = new BookWithDetailsSpec(searchKeyword, pageIndex, pageSize);

            var repo = _unitOfWork.GetRepository<Book, int>();

            return await repo.GetAllWithSpecAsync(spec);
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var spec = new BookWithDetailsSpec(id);
            var repo = _unitOfWork.GetRepository<Book, int>();

            return await repo.GetByIdWithSpecAsync(spec);
        }

        public async Task AddBookAsync(Book book)
        {
            var repo = _unitOfWork.GetRepository<Book, int>();
            await repo.AddAsync(book);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            var repo = _unitOfWork.GetRepository<Book, int>();
            repo.Update(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(Book book)
        {
            var repo = _unitOfWork.GetRepository<Book, int>();
            repo.Delete(book);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}