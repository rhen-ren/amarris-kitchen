namespace amarris_kitchen_backend.DTOs
{
    public class CreateOrderDTO
    {
        public string OrderMode { get; set; }
        public List<CreateOrderItemDTO> Items { get; set; }

    }
}
