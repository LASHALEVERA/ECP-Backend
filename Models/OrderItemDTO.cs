using Microsoft.EntityFrameworkCore;

namespace ECPAPI.Models
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        //public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } //
        public int quantity { get; set; }
        [Precision(18, 2)]
        public decimal price { get; set; }
    }
}

