using Domain.Contracts.SpecificationPattern;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Specifications
{
    public class BookWithDetailsSpec : BaseSpecifications<Book, int>
    {
        public BookWithDetailsSpec(string? searchKeyword, int pageIndex, int pageSize)
        {
            // Task 4: Include Category and Author (Single Database Trip)
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);

            // Task 15: Read-only query (No Tracking)
            ApplyNoTracking();

            // Task 11: Search by keyword (Case-Insensitive)
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                AddCriteria(b => b.Title.ToLower().Contains(searchKeyword.ToLower()));
            }

            // Task 12: Pagination
            ApplyPagenation(pageSize, pageIndex);
        }

        public BookWithDetailsSpec(int id) : base(b => b.Id == id)
        {
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);
            ApplyNoTracking();
        }
    }
}