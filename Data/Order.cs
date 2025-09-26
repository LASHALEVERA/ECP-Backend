using ECPAPI.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECPAPI.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        //public string Product {get; set; }  //
        //public enum status { Pending, Paid, Shipped, Delivered}  //
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public List<OrderItem> Items { get; set; } = new();

    }
    //public enum OrderStatus
    //{
    //    Pending,
    //    Paid,
    //    Shipped,
    //    Delivered
    //}
}

//public class Order
//{
//    public int Id { get; set; }
//    public int UserId { get; set; }
//    public string ShippingAddress { get; set; } = string.Empty;
//    public string PaymentMethod { get; set; } = string.Empty;
//    public DateTime CreatedAt { get; set; }

//    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
//}

//public class OrderItem
//{
//    public int Id { get; set; }
//    public int OrderId { get; set; }
//    public int ProductId { get; set; }
//    public int Quantity { get; set; }
//    public decimal Price { get; set; }

//    public Order Order { get; set; } = null!;
//}