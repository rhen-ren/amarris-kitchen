namespace amarris_kitchen_backend.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ? ImageUrl { get; set; }
    }
}

