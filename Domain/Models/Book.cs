namespace Domain.Models
{
    public abstract class Book : BaseEntity<int>
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
    public class Paperback : Book
    {
        public decimal ShippingWeight { get; set; }
    }

    public class EBook : Book
    {
        public string DownloadUrl { get; set; }
        public int FileSizeMB { get; set; }
    }
}
