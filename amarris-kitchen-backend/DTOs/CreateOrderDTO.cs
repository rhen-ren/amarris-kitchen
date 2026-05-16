namespace amarris_kitchen_backend.DTOs
{
    public class CreateOrderDTO
    {
        public string OrderMode { get; set; } = String.Empty;
        public List<CreateOrderItemDTO> ? Items { get; set; }

    }
}
