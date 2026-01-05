namespace TrendyProducts.DTOs
{
    public class CreateOrderDTO
    {
        public CustomerDTO Customer { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }

}
