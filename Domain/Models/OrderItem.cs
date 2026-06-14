
namespace Domain.Models
{
    public class OrderItem : BaseEntity<int>
    {
        public int Quantity { get; set; }
        public decimal UnitPriceAtPurchase { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
