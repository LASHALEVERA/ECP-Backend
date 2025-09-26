using ECPAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ECPAPI.Models
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        //public User User { get; set; }
        public UserDTO User { get; set; } //
        public List<OrderItemDTO> Items { get; set; } = new();
        //public string Product { get; set; }
        //public enum status { Pending, Paid, Shipped, Delivered }
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "Pending"; //
    }
}
