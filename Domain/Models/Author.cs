namespace Domain.Models
{
    public class Author : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
