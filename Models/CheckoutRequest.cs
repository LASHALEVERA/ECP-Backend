namespace ECPAPI.Models
{
    public class CheckoutRequest
    {
        public int UserId { get; set; }
        public string ShippingAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public List<OrderItemDTO> Items { get; set; } = new();
    }
    public class CheckoutItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
