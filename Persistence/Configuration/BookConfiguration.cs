using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("BOOKS");

            //TPH Pattern
            builder.HasDiscriminator<string>("BookType")
                   .HasValue<Paperback>("Paperback")
                   .HasValue<EBook>("EBook");

            // Business Rule
            builder.ToTable(b => b.HasCheckConstraint("CK_Book_Price_NotNegative", "Price >= 0"));

            // Business Rule
            builder.HasOne(b => b.Category)
                   .WithMany(c => c.Books)
                   .HasForeignKey(b => b.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
