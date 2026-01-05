namespace TrendyProducts.DTOs
{
    public class AdminOrderItemDTO
    {
        public string Title { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }

}
