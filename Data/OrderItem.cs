using Microsoft.EntityFrameworkCore;

namespace ECPAPI.Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        //public int OrderItemId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }

    }
}